using Serendipity.Domain.Contracts;
using Serendipity.Domain.Models;

namespace Serendipity.Domain.Interfaces.Services;

public interface IUserService
{
    public Task<IResult> FindUserById(string id);
    public Task<IResult> UpdateUser(User updateUser);
}