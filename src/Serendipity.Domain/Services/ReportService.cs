using Amazon;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Serendipity.Domain.Contracts;
using Serendipity.Domain.Interfaces.Services;
using Serendipity.Domain.Models;

namespace Serendipity.Domain.Services;

public class ReportService : IReportService
{
    private readonly AmazonS3Client _awsS3Service;
    private readonly string _bucket;
    private readonly string _reportFolderName;

    public ReportService(IConfiguration configuration)
    {
        var accessKey = configuration["AWS:AccessKey"];
        var secretKey= configuration["AWS:SecretKey"];
        _bucket = configuration["AWS:S3Bucket"];
        _reportFolderName = configuration["AWS:ReportFolder"];

        _awsS3Service = new AmazonS3Client(
            accessKey,
            secretKey,
            region: RegionEndpoint.EUWest1
        );
    }

    public async Task<IResult> GetReports()
    {
        try
        {
            var s3Objects = await _awsS3Service.ListObjectsAsync(
                _bucket, 
                _reportFolderName
            );

            var reports  = s3Objects.S3Objects
                .Where(e => e.Key != $"{_reportFolderName}/")    
                .Select(e => new Report
                {
                    Name = e.Key.Replace($"{_reportFolderName}/", String.Empty),
                    Link = $"/api/v1/Reports/{e.Key.Replace($"{_reportFolderName}/", String.Empty)}",
                    GeneratedAt = e.LastModified
                });

            return new SuccessResult<IEnumerable<Report>>(reports);
        }
        catch (Exception e)
        {
            return new ErrorResult("Sorry, but something went wrong when try to get reports");
        }
        
    }

    public void DownloadFile(string filename)
    {
        throw new NotImplementedException();
    }
}