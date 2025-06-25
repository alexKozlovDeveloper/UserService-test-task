using HomeTask.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeTask.Core.Infrastructure.Database;

public class HomeTaskDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public HomeTaskDbContext(DbContextOptions<HomeTaskDbContext> options) : base(options)
    {

    }
}
