using System.Reflection.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Serendipity.Domain.Models;
using Serendipity.Infrastructure.Models;
using Device = Serendipity.Infrastructure.Models.Device;
using User = Serendipity.Infrastructure.Models.User;

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
            optionsBuilder.UseNpgsql();
        }
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Device>(device =>
        {
            device.HasKey(d => d.Id);
            device.Property(d => d.Id);
            device.HasOne<User>()
                .WithMany(u => u.Devices)
                .HasForeignKey(d => d.UserId);
        });

        builder.Entity<PersonalInfo>(info =>
        {
            info.HasOne<User>()
                .WithOne(u => u.PersonalInfo)
                .HasForeignKey<PersonalInfo>(p => p.UserId);
        });

        builder.Entity<EmergencyContact>(ec =>
        {
            ec.HasOne<User>()
                .WithMany(u => u.EmergencyContacts)
                .HasForeignKey(c => c.UserId);
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