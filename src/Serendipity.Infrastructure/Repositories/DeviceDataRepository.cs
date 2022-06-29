using System.Net;
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
            new() { Name = "region", Value = "eu-west-1" },
            new() { Name = "az", Value = "az1" },
            new() { Name = "hostname", Value = "host1" }
        };

        var records = typeof(DeviceData).GetProperties()
            .Select(prop => new Record
            {
                Dimensions = dimensions,
                MeasureName = prop.Name,
                MeasureValue = prop.GetMethod!.Invoke(data.Data, Array.Empty<object>())!.ToString(),
                MeasureValueType = MeasureValueType.BIGINT,
                Time = data.Timestamp.ToString()
            })
            .ToList();

        try
        {
            var writeRecordsRequest = new WriteRecordsRequest
            {
                DatabaseName = _configuration["TimeStream::Database"],
                TableName = _configuration["TimeStream::Table"],
                Records = records
            };
            var response = await _writeClient.WriteRecordsAsync(writeRecordsRequest);
            if (response.HttpStatusCode != HttpStatusCode.OK)
            {
                return new ErrorResult(response.HttpStatusCode.ToString());
            }
            return new SuccessResult();
        }
        catch (RejectedRecordsException e)
        {
            return new ErrorResult(e.Message);
        }
        catch (Exception e)
        {
            return new ErrorResult(e.Message);
        }
    }
}