using System.Net;
using System.Text.Json;
using Amazon.TimestreamWrite;
using Amazon.TimestreamWrite.Model;
using Serendipity.Domain.Contracts;
using Serendipity.Domain.Interfaces.Repository;
using Serendipity.Domain.Models;
using Serendipity.Infrastructure.Database;

namespace Serendipity.Infrastructure.Repositories;

public class AlarmsRepository : IAlarmsRepository
{
    private readonly AppDbContext _db;
    
    public AlarmsRepository(AppDbContext db)
    {
        _db = db;
    }
    public Task<Result<IEnumerable<Alarm>>> GetLatest(string userEmail)
    {
        throw new NotImplementedException();
    }

    public async Task Insert(Alarm alarm)
    {
        await _db.Alarms.AddAsync(Map(alarm));
        await _db.SaveChangesAsync();

    }

    private static Infrastructure.Models.Alarm Map(Alarm domainAlarm)
    {
        return domainAlarm switch
        {
            FallAlarm a => new Infrastructure.Models.FallAlarm()
            {
                DeviceId = a.DeviceId,
                Timestamp = a.Timestamp,
                Type = a.Type
            },
            HeartBeatAlarm a => new Infrastructure.Models.HeartBeatAlarm()
            {
                DeviceId = a.DeviceId,
                Timestamp = a.Timestamp,
                HeartBeat = a.HeartBeat,
                Type = a.Type
            },
            LowBatteryAlarm a => new Infrastructure.Models.LowBatteryAlarm()
            {
                DeviceId = a.DeviceId,
                Timestamp = a.Timestamp,
                BatteryCharge = a.BatteryCharge,
                Type = a.Type
            },
            _ => throw new Exception("Mapper failed with invalid type.")
        };
    }
}