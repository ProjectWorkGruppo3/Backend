using Microsoft.EntityFrameworkCore;
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

    public Task<int> GetDeviceTotalAlarms(Guid deviceId)
    {
        return _db.Alarms
            .Where(el => el.DeviceId == deviceId.ToString())
            .CountAsync();
    }

    public async Task<IEnumerable<Alarm>> GetDeviceAlarms(Guid deviceId, int start, int limit)
    {
        
        var alarms = await  _db.Alarms
            .Where(el => el.DeviceId == deviceId.ToString())
            .Skip(start)
            .Take(limit)
            .Select(el => GetDomainAlarm(el))
            .ToListAsync();

        return alarms;
    }

    public async Task Insert(Alarm alarm)
    {
        await _db.Alarms.AddAsync(GetInfrastructureAlarm(alarm));
        await _db.SaveChangesAsync();

    }

    private static Infrastructure.Models.Alarm GetInfrastructureAlarm(Alarm domainAlarm)
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
            _ => throw new Exception("Mapper to infrastructure failed with invalid type.")
        };
    }

    private static Alarm GetDomainAlarm(Infrastructure.Models.Alarm infrastructureAlarm)
    {
        return infrastructureAlarm switch
        {
            Infrastructure.Models.FallAlarm a => new FallAlarm()
            {
                Timestamp = a.Timestamp,
                Type = a.Type,
                DeviceId = a.DeviceId
            },
            Infrastructure.Models.HeartBeatAlarm a => new HeartBeatAlarm()
            {
                Timestamp = a.Timestamp,
                Type = a.Type,
                DeviceId = a.DeviceId,
                HeartBeat = a.HeartBeat
            },
            Infrastructure.Models.LowBatteryAlarm a => new LowBatteryAlarm()
            {
                Timestamp = a.Timestamp,
                Type = a.Type,
                DeviceId = a.DeviceId,
                BatteryCharge = a.BatteryCharge
            },
            _ => throw new Exception("Mapper to domain failed with invalid type.")
        };
    }
}