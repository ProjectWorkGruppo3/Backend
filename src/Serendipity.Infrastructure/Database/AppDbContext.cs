using System.Reflection.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Serendipity.Domain.Models;
using Device = Serendipity.Infrastructure.Models.Device;

namespace Serendipity.Infrastructure.Database;

public class AppDbContext : IdentityDbContext<User, IdentityRole, string>
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public AppDbContext()
    {
    }

    public DbSet<Device> Devices { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Server=clod2021projectworkg3.c9nj1x2p6gk5.eu-west-1.rds.amazonaws.com;Port=5432;Database=EFTest;User Id=postgres;Password=0987654321;");
        }
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Device>(device =>
        {
            device.HasKey(d => d.Id);
            device.Property(d => d.Id);
        });
        
        base.OnModelCreating(builder);
    }
}

public class BloggingContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        return new AppDbContext();
    }
}