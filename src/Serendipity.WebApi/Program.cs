using System.Reflection;
using System.Text;
using Amazon;
using Amazon.S3;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serendipity.Domain.Defaults;
using Serendipity.Domain.Interfaces.Repository;
using Serendipity.Domain.Interfaces.Services;
using Serendipity.Domain.Models;
using Serendipity.Domain.Services;
using Serendipity.Infrastructure.Database;
using Serendipity.Infrastructure.Repositories;
using Serendipity.WebApi.Extensions;
using Serendipity.WebApi.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true);

builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<IDeviceService, DeviceService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<InputValidationActionFilter>();
builder.Services.AddScoped((serviceProvider) =>
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

builder.Services.AddIdentityCore<Serendipity.Infrastructure.Models.User>(options =>
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
