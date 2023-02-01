using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository;

public class StaticUserRepository : IUserRepository
{
    private List<User> _users = new List<User>
    {
        new User
        {
            FirstName = "Read Only",
            LastName = "User",
            EmailAddress = "readonly@user.com",
            Id = Guid.NewGuid(),
            Username = "readonly@user.com",
            Password = "readonly@user.com"
        },
        new User
        {
            FirstName = "Read Write",
            LastName = "User",
            EmailAddress = "readwrite@user.com",
            Id = Guid.NewGuid(),
            Username = "readwrite@user.com",
            Password = "readwrite@user.com"
        }
    };
    
    public async Task<User> AuthenticateUserAsync(string username, string password)
    {
        return _users
            .Find(x => x.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase) && x.Password.Equals(password));
    }
}