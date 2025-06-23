// See https://aka.ms/new-console-template for more information
using HomeTask.Core.Infrastructure.Database;
using HomeTask.Core.Models;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Home Task");

var db = new HomeTaskDbContext();

var user = new User 
{ 
    Name = "name",
    Email = "email",
    PasswordHash = "gr46#",
    Role = UserRole.User
};

db.Users.Add(user);
await db.SaveChangesAsync();

var users = await db.Users.ToListAsync();

foreach (var item in users)
{
    Console.WriteLine(item.Name);
}