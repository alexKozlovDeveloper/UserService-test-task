using HomeTask.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeTask.Core.Infrastructure.Database;

public class HomeTaskDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    //public string DbPath { get; } = string.Empty;

    public HomeTaskDbContext(DbContextOptions<HomeTaskDbContext> options) : base(options)
    {

    }

    //public HomeTaskDbContext()
    //{
    //    var folder = Environment.SpecialFolder.LocalApplicationData;
    //    var path = Environment.GetFolderPath(folder);
    //    DbPath = Path.Join(path, "hometask.db");
    //}

    //// The following configures EF to create a Sqlite database file in the
    //// special "local" folder for your platform.
    //protected override void OnConfiguring(DbContextOptionsBuilder options)
    //    => options.UseSqlite($"Data Source={DbPath}");
}
