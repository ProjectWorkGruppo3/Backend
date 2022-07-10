using Microsoft.EntityFrameworkCore;
using Serendipity.Domain.Contracts;
using Serendipity.Domain.Interfaces.Repository;
using Serendipity.Infrastructure.Database;
using Serendipity.Infrastructure.Models;
using User = Serendipity.Domain.Models.User;

namespace Serendipity.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IResult> FindUserById(string id)
    {
        
        var user = await _db.Users.Where(u=>u.Id == id).Include(u => u.PersonalInfo).SingleOrDefaultAsync();

        if (user is null)
        {
            return new NotFoundResult("User not found");
        }

        return new SuccessResult<User?>(new User
        {
            Email = user.Email,
            Name = user.Name,
            Surname = user.Surname,
            DayOfBirth = user.PersonalInfo?.BirthDay,
            Height = user.PersonalInfo?.Height,
            Weight = user.PersonalInfo?.Weight,
            Job = user.PersonalInfo?.Job
        });
    }


    public async Task<IResult> UpdateUser(User updateUser)
    {
        try
        {
            var user = await _db.Users
                .Where(u=>u.Id == updateUser.Id.ToString())
                .Include(u => u.PersonalInfo)
                .Include(u => u.EmergencyContacts)
                .SingleAsync()!;
            user.Email = updateUser.Email;
            user.Name = updateUser.Name;
            user.Surname = updateUser.Surname;
            user.PersonalInfo = new PersonalInfo
            {
                Height = updateUser.Height!.Value,
                Weight = updateUser.Weight!.Value,
                Job = updateUser.Job,
                BirthDay = updateUser.DayOfBirth!.Value
            };


            user.EmergencyContacts.Clear();

            await _db.SaveChangesAsync();
        
            user.EmergencyContacts = updateUser.EmergencyContacs.Select(el => new EmergencyContact
            {
                Email = el
            }).ToList();

            await _db.SaveChangesAsync();

            return new SuccessResult<User>(new User
            {
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                DayOfBirth = user.PersonalInfo?.BirthDay,
                Height = user.PersonalInfo?.Height,
                Weight = user.PersonalInfo?.Weight,
                Job = user.PersonalInfo?.Job
            });
        }
        catch (Exception e)
        {
            return new ErrorResult(e.Message);
        }
    }

    public async Task<int> GetNumberOfAdmins()
    {
        var adminRole = await _db.Roles.Where(e => e.Name == "Admin").SingleOrDefaultAsync();

        if (adminRole == null)
        {
            throw new Exception("");
        }

        var totalAdmins = await _db.UserRoles.Where(e => e.RoleId == adminRole.Id).CountAsync();

        return totalAdmins;
    }


    public async Task<IEnumerable<string>> GetUserEmergencyContactsFromDeviceId(Guid id)
    {
        var device = await _db.Devices.SingleOrDefaultAsync(el => el.Id == id);

        if (device == null)
        {
            return new List<string>();
        }

        var user = await _db.Users
            .Include(e => e.EmergencyContacts)
            .SingleAsync(el => el.Id == device.UserId);

        return user.EmergencyContacts.Select(el => el.Email);
    }
}