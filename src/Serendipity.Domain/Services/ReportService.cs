﻿using System.Net;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Serendipity.Domain.Contracts;
using Serendipity.Domain.Interfaces.Repository;
using Serendipity.Domain.Interfaces.Services;
using Serendipity.Domain.Models;

namespace Serendipity.Domain.Services;

public class ReportService : IReportService
{
    
    private readonly IReportRepository _repo;

    public ReportService(IReportRepository repo)
    {
        _repo = repo;
    }

    public async Task<IResult> GetReports()
    {
        return await _repo.GetReports();        
    }

    public async Task<IResult> DownloadFile(string filename)
    {
        return await _repo.DownloadFile(filename);        
    }
}