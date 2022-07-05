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
    public DbSet<GlobalStatistics> GlobalStatistics { get; set; } = null!;

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
        builder.Entity<GlobalStatistics>(stats =>
        {
            stats.HasKey(s => s.Date);
            stats.HasIndex(s => s.Date);
            stats.Property(s => s.Date)
                .HasColumnType("timestamp with time zone");

            stats.Property(s => s.LocationDensity)
                .HasColumnType("jsonb");
        });
        
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
            info.HasKey(i => i.UserId);
            info.HasOne<User>()
                .WithOne(u => u.PersonalInfo)
                .HasForeignKey<PersonalInfo>(p => p.UserId);
        });

        builder.Entity<EmergencyContact>(ec =>
        {
            ec.HasKey(e => e.Id);

            ec.Property(e => e.Id).ValueGeneratedOnAdd();
            
            ec.HasOne<User>()
                .WithMany(u => u.EmergencyContacts)
                .HasForeignKey(c => c.UserId);
        });

        builder.Entity<User>(user =>
        {
            user.HasMany(u => u.EmergencyContacts)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId);

            user.HasOne(u => u.PersonalInfo)
                .WithOne(p => p.User)
                .HasForeignKey<PersonalInfo>(p => p.UserId);
        });
        
        base.OnModelCreating(builder);
    }
}

/*public class BloggingContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        return new AppDbContext();
    }
}*/