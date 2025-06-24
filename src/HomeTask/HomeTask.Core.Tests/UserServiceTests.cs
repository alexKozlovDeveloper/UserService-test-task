using HomeTask.Core.Infrastructure.Database;
using HomeTask.Core.Interfaces;
using HomeTask.Core.Interfaces.Implementations;
using HomeTask.Core.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace HomeTask.Core.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task GetUsersAsync_ReturnListOfUsers()
        {
            // Arrange
            var hashServiceMock = new Mock<IPasswordHashService>();
            var userNotificationService = new Mock<IUserNotificationService>();

            var user1 = new User { Name = "Alex", Email = "Alex@email.com", PasswordHash = "Gt9Yc4AiI", Role = UserRole.User };
            var user2 = new User { Name = "Jon", Email = "Jon@email.com", PasswordHash = "vmsC1QQbe2R", Role = UserRole.User };
            var user3 = new User { Name = "Sam", Email = "Sam@email.com", PasswordHash = "ZsCIqvoYlst2x", Role = UserRole.Admin };

            var options = new DbContextOptionsBuilder<HomeTaskDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            using var context = new HomeTaskDbContext(options);

            context.Users.AddRange(user1, user2, user3);

            await context.SaveChangesAsync();

            var userService = new UserService(hashServiceMock.Object, userNotificationService.Object, context);

            // Act
            var result = await userService.GetUsersAsync(default);

            // Assert
            Assert.Equal(3, result.Count);

            EqualUser(user1, result[0]);
            EqualUser(user2, result[1]);
            EqualUser(user3, result[2]);

            context.Dispose();
        }

        [Fact]
        public async Task GetUsersAsync_ReturnListOfUsers()
        {
            // Arrange
            var hashServiceMock = new Mock<IPasswordHashService>();
            var userNotificationService = new Mock<IUserNotificationService>();

            var user1 = new User { Name = "Alex", Email = "Alex@email.com", PasswordHash = "Gt9Yc4AiI", Role = UserRole.User };
            var user2 = new User { Name = "Jon", Email = "Jon@email.com", PasswordHash = "vmsC1QQbe2R", Role = UserRole.User };
            var user3 = new User { Name = "Sam", Email = "Sam@email.com", PasswordHash = "ZsCIqvoYlst2x", Role = UserRole.Admin };

            var options = new DbContextOptionsBuilder<HomeTaskDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            using var context = new HomeTaskDbContext(options);

            context.Users.AddRange(user1, user2, user3);

            await context.SaveChangesAsync();

            var userService = new UserService(hashServiceMock.Object, userNotificationService.Object, context);

            // Act
            var result = await userService.GetUsersAsync(default);

            // Assert
            Assert.Equal(3, result.Count);

            EqualUser(user1, result[0]);
            EqualUser(user2, result[1]);
            EqualUser(user3, result[2]);

            context.Dispose();
        }

        private void EqualUser(User expected, UserDto result) 
        {
            Assert.Equal(expected.Name, result.Name);
            Assert.Equal(expected.Role, result.Role);
        }
    }
}