using Serendipity.Domain.Contracts;
using Serendipity.Domain.Models;

namespace Serendipity.Domain.Interfaces.Repository;

public interface IUserRepository
{
    public Task<IResult> FindUserById(string id);
    public Task<IResult> UpdateUser(User updateUser);

    public Task<int> GetNumberOfAdmins();

    public Task<IEnumerable<string>> GetUserEmergencyContactsFromDeviceId(Guid id);
}