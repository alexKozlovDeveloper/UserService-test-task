using HomeTask.Core.Entities;
using HomeTask.Core.Infrastructure.Database;
using HomeTask.Core.Models.Request;
using HomeTask.Core.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace HomeTask.Core.Interfaces.Implementations;

public class UserService(
    IPasswordHashService PasswordHashService,
    IUserNotificationService UserNotificationService,
    HomeTaskDbContext DbContext
    ) : IUserService
{
    public async Task<int> CreateUserAsync(CreateUserRequestModel model, CancellationToken ct)
    {
        model.Validate();

        var user = new User
        {
            Name = model.Name,
            Email = model.Email,
            PasswordHash = PasswordHashService.Hash(model.Password),
            Role = model.Role
        };

        DbContext.Users.Add(user);

        await DbContext.SaveChangesAsync(ct);

        return user.Id;
    }

    public async Task<IList<UserResponseModel>> GetUsersAsync(CancellationToken ct)
    {
        var users = await DbContext.Users
            .Select(x => new UserResponseModel
            {
                Id = x.Id,
                Name = x.Name,
                Role = x.Role
            })
            .ToListAsync(ct);

        return users;
    }

    public async Task UpdateUserRoleAsync(int userId, UserRole newRole, CancellationToken ct)
    {
        var user = await DbContext.Users
            .Where(x => x.Id == userId)
            .FirstOrDefaultAsync(ct);

        if (user == null) 
        {
            throw new Exception($"User with id '{userId}' not found");
        }

        user.Role = newRole;

        await DbContext.SaveChangesAsync(ct);

        await UserNotificationService.NotifyUserUpdatedAsync(new UserResponseModel 
        { 
            Id = user.Id, 
            Name = user.Name, 
            Role = user.Role 
        }, ct);
    }
}
