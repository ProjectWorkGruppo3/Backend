using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Serendipity.Domain.Contracts;
using Serendipity.Domain.Interfaces.Repository;
using Serendipity.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Serendipity.Infrastructure.Repositories;
public class ReportRepository : IReportRepository
{
    private readonly AmazonS3Client _awsS3Service;
    private readonly string _bucket;
    private readonly string _reportFolderName;

    public ReportRepository(IConfiguration configuration)
    {
        var accessKey = configuration["AWS:AccessKey"];
        var secretKey = configuration["AWS:SecretKey"];
        _bucket = configuration["AWS:S3Bucket"];
        _reportFolderName = configuration["AWS:ReportFolder"];

        _awsS3Service = new AmazonS3Client(
            accessKey,
            secretKey,
            region: RegionEndpoint.EUWest1
        );
    }

    public async Task<IResult> DownloadFile(string filename)
    {
        try
        {
            var ms = new MemoryStream();

            var getObject = await _awsS3Service.GetObjectAsync(new GetObjectRequest
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

    public async Task<IResult> GetReports()
    {
        try
        {
            var s3Objects = await _awsS3Service.ListObjectsAsync(
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

            return new SuccessResult<IEnumerable<Report>>(reports);
        }
        catch (Exception)
        {
            return new ErrorResult("Sorry, but something went wrong when try to get reports");
        }
    }
}
