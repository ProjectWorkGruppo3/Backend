using System.Net;
using System.Text.Json;
using Amazon.TimestreamQuery;
using Amazon.TimestreamQuery.Model;
using Amazon.TimestreamWrite;
using Amazon.TimestreamWrite.Model;
using Microsoft.Extensions.Configuration;
using Serendipity.Domain.Contracts;
using Serendipity.Domain.Interfaces.Repository;
using Serendipity.Domain.Models;
using MeasureValueType = Amazon.TimestreamWrite.MeasureValueType;

namespace Serendipity.Infrastructure.Repositories;

public class DeviceDataRepository : IDeviceDataRepository
{
    private readonly AmazonTimestreamWriteClient _writeClient;
    private readonly AmazonTimestreamQueryClient _readClient;

    private readonly string _databaseName;
    private readonly string _tableName;
    

    public DeviceDataRepository(AmazonTimestreamWriteClient writeClient, AmazonTimestreamQueryClient readClient, string databaseName, string tableName)
    {
        _writeClient = writeClient;
        _readClient = readClient;
        _databaseName = databaseName;
        _tableName = tableName;
    }

    public async Task<IResult> Insert(DeviceDataModel data)
    {

        var dimensions = new List<Dimension>{
            new() { Name = "data", Value = "bracelet" }
        };

        List<Record> records = new()
        {
            new Record
            {
                Dimensions = dimensions,
                MeasureName = "info",
                MeasureValue = JsonSerializer.Serialize(data, new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
                }),
                MeasureValueType = MeasureValueType.VARCHAR,
                Time = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString()
            }
        };
        
        try
        {
            var writeRecordsRequest = new WriteRecordsRequest
            {
                DatabaseName = _databaseName,
                TableName = _tableName,
                Records = records
            };
            var response = await _writeClient.WriteRecordsAsync(writeRecordsRequest);
            if (response.HttpStatusCode != HttpStatusCode.OK)
            {
                return new ErrorResult(response.HttpStatusCode.ToString());
            }
            return new SuccessResult();
        }
        catch (Exception e)
        {
            return new ErrorResult(e.Message);
        }
    }

    public async Task<IEnumerable<DeviceDataModel>> GetLatestDeviceData(string deviceId)
    {
        var readRecordRequest = new QueryRequest
        {
            QueryString = @$"
                SELECT 
                    *
                FROM 
                    ""{_databaseName}"".""{_tableName}"" 
                WHERE
                    ""measure_value::varchar"" LIKE '%{deviceId}%'
                ORDER BY time DESC
                LIMIT 2
"
        };

        var queryResponse = await _readClient.QueryAsync(readRecordRequest);
        var latestDeviceData = ParseQueryResult(queryResponse);

        
        return latestDeviceData;
        
    }

    public async Task<IEnumerable<AnalyticsChartData>> GetSerendipityChartData(string deviceId)
    {
        var readRecordRequest = new QueryRequest
        {
            QueryString = @$"
                SELECT 
                    *
                FROM 
                    ""{_databaseName}"".""{_tableName}"" 
                WHERE
                    time between ago(31d) and now() 
                    and
                    ""measure_value::varchar"" LIKE '%{deviceId}%'
                ORDER BY 
                    time
"
        };

        var queryResponse = await _readClient.QueryAsync(readRecordRequest);
        
        var latestDeviceData = ParseQueryResult(queryResponse);


        return latestDeviceData.Select(el => new AnalyticsChartData
        {
            Date = el.Timestamp,
            Value = el.Data.Serendipity
        });
    }
    
    public async Task<IEnumerable<AnalyticsChartData>> GetFallsChartData(string deviceId)
    {
        var readRecordRequest = new QueryRequest
        {
            QueryString = @$"
                SELECT 
                    *
                FROM 
                    ""{_databaseName}"".""{_tableName}"" 
                WHERE
                    time between ago(31d) and now() 
                    and
                    ""measure_value::varchar"" LIKE '%{deviceId}%'
                ORDER BY 
                    time
"
        };

        var queryResponse = await _readClient.QueryAsync(readRecordRequest);
        
        var latestDeviceData = ParseQueryResult(queryResponse);


        return latestDeviceData.Select(el => new AnalyticsChartData
        {
            Date = el.Timestamp,
            Value = el.Data.Serendipity
        });
    }

    public async Task<IEnumerable<AnalyticsChartData>> GetHeartbeatChartData(string deviceId)
    {
        var readRecordRequest = new QueryRequest
        {
            QueryString = @$"
                SELECT 
                    *
                FROM 
                    ""{_databaseName}"".""{_tableName}"" 
                WHERE
                    time between ago(31d) and now() 
                    and
                    ""measure_value::varchar"" LIKE '%{deviceId}%'
                ORDER BY 
                    time
"
        };

        var queryResponse = await _readClient.QueryAsync(readRecordRequest);
        
        var latestDeviceData = ParseQueryResult(queryResponse);


        return latestDeviceData.Select(el => new AnalyticsChartData
        {
            Date = el.Timestamp,
            Value = el.Data.Heartbeat
        });
    }

    public async Task<IEnumerable<AnalyticsChartData>> GetStandingsChartData(string deviceId)
    {
        var readRecordRequest = new QueryRequest
        {
            QueryString = @$"
                SELECT 
                    *
                FROM 
                    ""{_databaseName}"".""{_tableName}"" 
                WHERE
                    time between ago(31d) and now() 
                    and
                    ""measure_value::varchar"" LIKE '%{deviceId}%'
                ORDER BY 
                    time
"
        };

        var queryResponse = await _readClient.QueryAsync(readRecordRequest);
        
        var latestDeviceData = ParseQueryResult(queryResponse);


        return latestDeviceData.Select(el => new AnalyticsChartData
        {
            Date = el.Timestamp,
            Value = el.Data.Standings
        });
    }

    public async Task<IEnumerable<AnalyticsChartData>> GetStepsChartData(string deviceId)
    {
        var readRecordRequest = new QueryRequest
        {
            QueryString = @$"
                SELECT 
                    *
                FROM 
                    ""{_databaseName}"".""{_tableName}"" 
                WHERE
                    time between ago(31d) and now() 
                    and
                    ""measure_value::varchar"" LIKE '%{deviceId}%'
                ORDER BY 
                    time
"
        };

        var queryResponse = await _readClient.QueryAsync(readRecordRequest);
        
        var latestDeviceData = ParseQueryResult(queryResponse);


        return latestDeviceData.Select(el => new AnalyticsChartData
        {
            Date = el.Timestamp,
            Value = el.Data.StepsWalked
        });
    }


    private List<DeviceDataModel> ParseQueryResult(QueryResponse response)
    {
        var columnInfo = response.ColumnInfo;
        var rows = response.Rows;

        var l = new List<DeviceDataModel>();
        
        foreach (var row in rows)
        {
            var json = ParseRow(columnInfo, row);

            
            var parsed = JsonSerializer.Deserialize<DeviceDataModel>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            if(parsed != null)
                l.Add(parsed);
        }

        return l;
    }
    
    private string ParseRow(List<ColumnInfo> columnInfo, Row row)
    {
        List<Datum> data = row.Data;

        var index = columnInfo.Select(e => e.Name).ToList().IndexOf("measure_value::varchar");
        
        return data[index].ScalarValue;
    }
    
    
}