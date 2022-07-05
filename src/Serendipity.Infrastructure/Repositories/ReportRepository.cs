using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Serendipity.Domain.Contracts;
using Serendipity.Domain.Interfaces.Repository;
using Serendipity.Domain.Models;
using System.Net;

namespace Serendipity.Infrastructure.Repositories;
public class ReportRepository : IReportRepository
{
    private readonly AmazonS3Client _amazonS3Client;
    private readonly string _bucket;
    private readonly string _reportFolderName;

    public ReportRepository(AmazonS3Client amazonS3Client, IConfiguration configuration)
    {
        _amazonS3Client = amazonS3Client;
        _bucket = configuration["AWS:S3Bucket"];
        _reportFolderName = configuration["AWS:ReportFolder"];
    }

    public async Task<IEnumerable<Report>> GetLatestReports(int count)
    {
        var s3Objects = await _amazonS3Client.ListObjectsAsync(
            _bucket,
            _reportFolderName
        );

        var reports = s3Objects.S3Objects
            .Where(e => e.Key != $"{_reportFolderName}/")
            .Take(count)
            .Select(e => new Report
            {
                Name = e.Key.Replace($"{_reportFolderName}/", string.Empty),
                Link = $"/api/v1/Reports/{e.Key.Replace($"{_reportFolderName}/", string.Empty)}",
                GeneratedAt = e.LastModified
            });
        
        

        return reports;
    }

    public async Task<IResult> DownloadFile(string filename)
    {
        try
        {
            var ms = new MemoryStream();

            var getObject = await _amazonS3Client.GetObjectAsync(new GetObjectRequest
            {
                BucketName = _bucket,
                Key = $"{_reportFolderName}/{filename}"
            });

            if (getObject.HttpStatusCode == HttpStatusCode.NotFound)
            {
                return new NotFoundResult(filename);
            }


            if (getObject.HttpStatusCode == HttpStatusCode.OK)
            {
                await getObject.ResponseStream.CopyToAsync(ms);
            }

            return new SuccessResult<byte[]>(ms.ToArray());

        }
        catch (Exception e)
        {
            return new ErrorResult(e.Message);
        }
    }

    public async Task<IEnumerable<Report>> GetReports()
    {
        var s3Objects = await _amazonS3Client.ListObjectsAsync(
            _bucket,
            _reportFolderName
        );

        var reports = s3Objects.S3Objects
            .Where(e => e.Key != $"{_reportFolderName}/")
            .Select(e => new Report
            {
                Name = e.Key.Replace($"{_reportFolderName}/", string.Empty),
                Link = $"/api/v1/Reports/{e.Key.Replace($"{_reportFolderName}/", string.Empty)}",
                GeneratedAt = e.LastModified
            });

        return reports;
    }
}
