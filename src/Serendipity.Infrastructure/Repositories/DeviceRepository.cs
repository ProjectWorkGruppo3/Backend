using Microsoft.EntityFrameworkCore;
using Serendipity.Domain.Contracts;
using Serendipity.Domain.Interfaces.Repository;
using Serendipity.Infrastructure.Database;
using Serendipity.Infrastructure.Models;

namespace Serendipity.Infrastructure.Repositories;

public class DeviceRepository : IDeviceRepository
{
    private readonly AppDbContext _db;

    public DeviceRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<int> GetTotalNumberDevices()
    {
        var count = await _db.Devices.CountAsync();

        return count;
    }

    public async Task<IEnumerable<Domain.Models.Device>> GetUserDevices(string userId)
    {
        var userDevices = await _db.Devices.Where(d => d.UserId == userId).ToListAsync();


        return userDevices.Select(e => new Domain.Models.Device 
            {
                Id = e.Id,
                Name = e.Name,
                UserId = e.UserId
            });
    }

    public async Task<IResult> RegisterDevice(string userId, Guid deviceId, string name)
    {
        try
        {
            await _db.Devices.AddAsync(new Device
            {
                Id = deviceId,
                UserId = userId,
                Name = name
            });

            await _db.SaveChangesAsync();

            var created = await _db.Devices.FindAsync(deviceId);

            if (created is null) throw new Exception($"Creation failed at {nameof(DeviceRepository)}.");
            
            return new SuccessResult<Domain.Models.Device>(new Domain.Models.Device
            {
                Id = created.Id,
                UserId = created.UserId,
                Name = created.Name
            });
        }
        catch (Exception)
        {
            return new ErrorResult("Db error.");
        }
    }
}