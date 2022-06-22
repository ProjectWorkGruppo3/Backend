using Serendipity.Domain.Models;

namespace Serendipity.Domain.Interfaces.Repository;

public interface IUserRepository
{
    public Task<User?> FindUserById(string id);
}