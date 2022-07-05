using Serendipity.Domain.Contracts;
using Serendipity.Domain.Interfaces.Repository;
using Serendipity.Domain.Interfaces.Services;
using Serendipity.Domain.Models;

namespace Serendipity.Domain.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _users;

    public UserService(IUserRepository users)
    {
        _users = users;
    }

    public Task<IResult> FindUserById(string id)
    {
        return _users.FindUserById(id);
    }

    public Task<IResult> UpdateUser(User updateUser)
    {
        return _users.UpdateUser(updateUser);
    }
}