using System.Reflection;
using System.Text;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.TimestreamWrite;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using SendGrid.Extensions.DependencyInjection;
using Serendipity.Domain.Defaults;
using Serendipity.Domain.Interfaces.Providers;
using Serendipity.Domain.Interfaces.Repository;
using Serendipity.Domain.Interfaces.Services;
using Serendipity.Domain.Services;
using Serendipity.Infrastructure.Database;
using Serendipity.Infrastructure.Models;
using Serendipity.Infrastructure.Providers;
using Serendipity.Infrastructure.Repositories;
using Serendipity.WebApi.Extensions;
using Serendipity.WebApi.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true);

// Device
builder.Services.TryAddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.TryAddScoped<IDeviceService, DeviceService>();
// User
builder.Services.TryAddScoped<IUserService, UserService>();
builder.Services.TryAddScoped<IUserRepository, UserRepository>();
// Analysis
builder.Services.TryAddScoped<IAnalysisRepository, AnalysisRepository>();
builder.Services.TryAddScoped<IAnalysisService, AnalysisService>();
// Report
builder.Services.TryAddScoped<IReportService, ReportService>();
builder.Services.TryAddScoped<IReportRepository, ReportRepository>();
// DeviceData
builder.Services.TryAddScoped<IDeviceDataService, DeviceDataService>();
builder.Services.TryAddScoped<IDeviceDataRepository, DeviceDataRepository>();
// Email
builder.Services.TryAddScoped<IEmailProvider, EmailProvider>();
// Action Filter
builder.Services.TryAddScoped<InputValidationActionFilter>();

builder.Services.AddSendGrid((serviceProvider, options) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    options.ApiKey = configuration["Email:SendgridApiKey"] 
                     ?? Environment.GetEnvironmentVariable("SENDGRID_API_KEY") 
                     ?? throw new Exception("Missing Sendgrid Api Key");
});

builder.Services.AddScoped(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var accessKey = config["AWS:AccessKey"];
    var secretKey = config["AWS:SecretKey"];
    return new AmazonTimestreamWriteClient(
        new BasicAWSCredentials(accessKey, secretKey),
        RegionEndpoint.EUWest1);
});

builder.Services.AddScoped(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var accessKey = configuration["AWS:AccessKey"];
    var secretKey = configuration["AWS:SecretKey"];
    return new AmazonS3Client(
        accessKey,
        secretKey,
        region: RegionEndpoint.EUWest1
    );
});

builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default") ?? Environment.GetEnvironmentVariable("ConnectionString");

    if (connectionString == null)
    {
        throw new Exception("Connection String not provided");
    }
    
    
    
    options.UseNpgsql(connectionString);
});

builder.Services.AddIdentityCore<User>(options =>
    {
        options.User.RequireUniqueEmail = true;
    })
    .AddRoles<IdentityRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<AppDbContext>();



builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
        };
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicies.AllowAll, corsPolicyBuilder =>
    {
        corsPolicyBuilder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(CorsPolicies.AllowAll);

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.SeedDefaultUser();
app.Run();
