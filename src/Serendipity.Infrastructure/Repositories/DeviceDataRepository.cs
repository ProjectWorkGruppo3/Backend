using System.Net;
using System.Text.Json;
using Amazon.TimestreamWrite;
using Amazon.TimestreamWrite.Model;
using Microsoft.Extensions.Configuration;
using Serendipity.Domain.Contracts;
using Serendipity.Domain.Interfaces.Repository;
using Serendipity.Domain.Models;

namespace Serendipity.Infrastructure.Repositories;

public class DeviceDataRepository : IDeviceDataRepository
{
    private readonly AmazonTimestreamWriteClient _writeClient;
    private readonly IConfiguration _configuration;

    public DeviceDataRepository(AmazonTimestreamWriteClient writeClient, IConfiguration configuration)
    {
        _writeClient = writeClient;
        _configuration = configuration;
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
                DatabaseName = _configuration["TimeStream:Database"],
                TableName = _configuration["TimeStream:Table"],
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
}