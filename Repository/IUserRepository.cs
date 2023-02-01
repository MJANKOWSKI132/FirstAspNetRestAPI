using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository;

public interface IUserRepository
{
    Task<User> AuthenticateUserAsync(string username, string password);
}