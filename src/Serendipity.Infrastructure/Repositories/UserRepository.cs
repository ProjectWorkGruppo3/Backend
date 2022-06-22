using Microsoft.EntityFrameworkCore;
using Serendipity.Domain.Interfaces.Repository;
using Serendipity.Domain.Models;
using Serendipity.Infrastructure.Database;

namespace Serendipity.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<User?> FindUserById(string id)
    {
        
        var user = await _db.Users.Where(u=>u.Id == id).Include(u => u.PersonalInfo).SingleOrDefaultAsync();

        if (user is null) return null;
        
        return new User
        {
            Email = user.Email,
            Name = user.Name,
            Surname = user.Surname,
            DayOfBirth = user.PersonalInfo?.BirthDay,
            Height = user.PersonalInfo?.Height,
            Weight = user.PersonalInfo?.Weight
        };
    }
}