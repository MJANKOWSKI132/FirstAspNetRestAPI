using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository;

public class UserRepository : IUserRepository
{
    private readonly NZWalksDbContext _dbContext;

    public UserRepository(NZWalksDbContext dbContext)
    {
        this._dbContext = dbContext;
    }
    public async Task<User> AuthenticateUserAsync(string username, string password)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x =>
            x.Username.ToLower() == username.ToLower() && x.Password == password);
        if (user == null) return null;
        var userRoles = await _dbContext.UserRoles.Where(x => x.UserId == user.Id).ToListAsync();
        if (userRoles.Any())
        {
            user.Roles = new List<string>();
            foreach (var userRole in userRoles)
            {
                var role = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Id == userRole.RoleId);
                if (role != null)
                {
                    user.Roles.Add(role.Name);
                }
            }
        }

        user.Password = null;
        return user;
    }
}