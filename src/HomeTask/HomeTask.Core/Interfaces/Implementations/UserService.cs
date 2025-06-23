using HomeTask.Core.Infrastructure.Database;
using HomeTask.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeTask.Core.Interfaces.Implementations;

public class UserService(
    IPasswordHashService PasswordHashService,
    HomeTaskDbContext DbContext
    )
{
    public async Task<int> CreateUser(
        CreateUserDataModel model, CancellationToken ct
        //string name, string email, string password, string role
        )
    {
        //if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        //{
        //    throw new Exception("Invalid input");
        //}

        //var emailRegex = new System.Text.RegularExpressions.Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        //if (!emailRegex.IsMatch(email))
        //{
        //    throw new Exception("Invalid email");
        //}

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

        //var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

        //using (var db = new SqlConnection("connectionString"))
        //{
        //    db.Open();
        //    var command = new SqlCommand($"INSERT INTO Users (Name, Email, PasswordHash, Role) VALUES ('{name}', '{email}', '{passwordHash}', '{role}')", db);
        //    command.ExecuteNonQuery();
        //}
    }

    public async Task<List<UserDto>> GetUsersAsync(CancellationToken ct)
    {
        //var users = new List<string>();

        var users = await DbContext.Users
            .Select(x => new UserDto
            {
                Name = x.Name
            })
            .ToListAsync(ct);

        return users;

        //using (var db = new SqlConnection("connectionString"))
        //{
        //    db.Open();
        //    var command = new SqlCommand("SELECT Name FROM Users", db);
        //    using (var reader = command.ExecuteReader())
        //    {
        //        while (reader.Read())
        //        {
        //            users.Add(reader.GetString(0));
        //        }
        //    }
        //}

        //return users;
    }

    public async Task UpdateUserRole(int userId, UserRole newRole, CancellationToken ct)
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


        //if (newRole != "Admin" && newRole != "User")
        //{
        //    throw new Exception("Invalid role");
        //}

        //using (var db = new SqlConnection("connectionString"))
        //{
        //    db.Open();
        //    var command = new SqlCommand($"UPDATE Users SET Role = '{newRole}' WHERE Id = {userId}", db);
        //    command.ExecuteNonQuery();
        //}
    }
}
