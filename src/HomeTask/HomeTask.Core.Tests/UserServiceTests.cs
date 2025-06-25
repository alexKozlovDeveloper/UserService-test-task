using HomeTask.Core.Entities;
using HomeTask.Core.Infrastructure.Database;
using HomeTask.Core.Interfaces;
using HomeTask.Core.Interfaces.Implementations;
using HomeTask.Core.Models;
using HomeTask.Core.Models.Request;
using HomeTask.Core.Models.Response;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace HomeTask.Core.Tests;

public class UserServiceTests
{        
    private const string ValidName = "Jon";
    private const string ValidEmail = "Jon@email.com";
    private const string ValidPassword = "avcx@#241FD";

    [Fact]
    public async Task GetUsersAsync_ReturnListOfUsers()
    {
        // Arrange
        var hashServiceMock = new Mock<IPasswordHashService>();
        var userNotificationService = new Mock<IEventService>();

        var user1 = new User { Name = "Alex", Email = "Alex@email.com", PasswordHash = "Gt9Yc4AiI", Role = UserRole.User };
        var user2 = new User { Name = "Jon", Email = "Jon@email.com", PasswordHash = "vmsC1QQbe2R", Role = UserRole.User };
        var user3 = new User { Name = "Sam", Email = "Sam@email.com", PasswordHash = "ZsCIqvoYlst2x", Role = UserRole.Admin };

        var options = new DbContextOptionsBuilder<HomeTaskDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        using var context = new HomeTaskDbContext(options);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

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
    }

    [Fact]
    public async Task GetUsersAsync_SuccessfullyUpdated()
    {
        // Arrange
        var hashServiceMock = new Mock<IPasswordHashService>();
        var userNotificationService = new Mock<IEventService>();

        UserResponseModel? notifiedUser = null;

        userNotificationService
            .Setup(x => x.SendAsync(It.IsAny<UserUpdatedEvent>(), It.IsAny<CancellationToken>()))
            .Callback<UserUpdatedEvent, CancellationToken>((model, ct) => notifiedUser = model.User)
            .Returns(Task.CompletedTask);

        var user1 = new User { Name = "Alex", Email = "Alex@email.com", PasswordHash = "Gt9Yc4AiI", Role = UserRole.User };
        var user2 = new User { Name = "Jon", Email = "Jon@email.com", PasswordHash = "vmsC1QQbe2R", Role = UserRole.User };
        var user3 = new User { Name = "Sam", Email = "Sam@email.com", PasswordHash = "ZsCIqvoYlst2x", Role = UserRole.Admin };

        var options = new DbContextOptionsBuilder<HomeTaskDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        using var context = new HomeTaskDbContext(options);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        context.Users.AddRange(user1, user2, user3);

        await context.SaveChangesAsync();

        var userService = new UserService(hashServiceMock.Object, userNotificationService.Object, context);

        var userId = user2.Id;
        var newUserRole = UserRole.Admin;

        // Act
        await userService.UpdateUserRoleAsync(userId, newUserRole, default);

        // Assert            
        var updatedUser = context.Users
            .Where(x => x.Id == userId)
            .FirstOrDefault();

        Assert.NotNull(updatedUser);

        Assert.Equal(newUserRole, updatedUser.Role);

        Assert.NotNull(notifiedUser);

        Assert.Equal("Jon", notifiedUser.Name);
        Assert.Equal(newUserRole, notifiedUser.Role);
    }

    [Fact]
    public async Task GetUsersAsync_UserNotFound()
    {
        // Arrange
        var hashServiceMock = new Mock<IPasswordHashService>();
        var userNotificationService = new Mock<IEventService>();

        var user1 = new User { Name = "Alex", Email = "Alex@email.com", PasswordHash = "Gt9Yc4AiI", Role = UserRole.User };
        var user2 = new User { Name = "Jon", Email = "Jon@email.com", PasswordHash = "vmsC1QQbe2R", Role = UserRole.User };
        var user3 = new User { Name = "Sam", Email = "Sam@email.com", PasswordHash = "ZsCIqvoYlst2x", Role = UserRole.Admin };

        var options = new DbContextOptionsBuilder<HomeTaskDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        using var context = new HomeTaskDbContext(options);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        context.Users.AddRange(user1, user2, user3);

        await context.SaveChangesAsync();

        var userService = new UserService(hashServiceMock.Object, userNotificationService.Object, context);

        var userId = -1;
        var newUserRole = UserRole.Admin;
        var expectedErrorMessage = $"User with id '{userId}' not found";
        string resultErrorMessage = null;

        // Act
        try
        {
            await userService.UpdateUserRoleAsync(userId, newUserRole, default);
        }
        catch (Exception e)
        {
            resultErrorMessage = e.Message;
        }

        // Assert
        Assert.Equal(expectedErrorMessage, resultErrorMessage);
    }

    [Fact]
    public async Task CreateUserAsync_SuccessfullyCreated()
    {
        // Arrange
        var hashServiceMock = new Mock<IPasswordHashService>();
        var userNotificationService = new Mock<IEventService>();

        var passwordHash = "abc3";

        hashServiceMock
            .Setup(x => x.Hash(It.IsAny<string>()))
            .Returns(passwordHash);

        var user1 = new User { Name = "Alex", Email = "Alex@email.com", PasswordHash = "Gt9Yc4AiI", Role = UserRole.User };
        var user2 = new User { Name = "Jon", Email = "Jon@email.com", PasswordHash = "vmsC1QQbe2R", Role = UserRole.User };
        var user3 = new User { Name = "Sam", Email = "Sam@email.com", PasswordHash = "ZsCIqvoYlst2x", Role = UserRole.Admin };

        var options = new DbContextOptionsBuilder<HomeTaskDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        using var context = new HomeTaskDbContext(options);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        context.Users.AddRange(user1, user2, user3);

        await context.SaveChangesAsync();

        var userService = new UserService(hashServiceMock.Object, userNotificationService.Object, context);

        var createUserModel = new CreateUserRequestModel
        {
            Name = "Mike",
            Email = "Mike@email.com",
            Password = "avcx@#241FD",
            Role = UserRole.User
        };

        // Act
        var createdUserId = await userService.CreateUserAsync(createUserModel, default);

        // Assert
        var createdUser = context.Users
            .Where(x => x.Id == createdUserId)
            .FirstOrDefault();

        Assert.NotNull(createdUser);

        Assert.Equal(createUserModel.Name, createdUser.Name);
        Assert.Equal(createUserModel.Email, createdUser.Email);
        Assert.Equal(createUserModel.Role, createdUser.Role);
        Assert.Equal(passwordHash, createdUser.PasswordHash);
    }

    [Theory]
    [InlineData(ValidName, null, ValidPassword, "Validation failed: Email can't be empty")]
    [InlineData(ValidName, "", ValidPassword, "Validation failed: Email can't be empty")]
    [InlineData(ValidName, "abc.com", ValidPassword, "Validation failed: Invalid email")]
    [InlineData(ValidName, "@abc", ValidPassword, "Validation failed: Invalid email")]
    [InlineData("", ValidEmail, ValidPassword, "Validation failed: Name can't be empty")]
    [InlineData("  ", ValidEmail, ValidPassword, "Validation failed: Name can't be empty")]
    [InlineData(null, ValidEmail, ValidPassword, "Validation failed: Name can't be empty")]
    [InlineData(ValidName, ValidEmail, null, "Validation failed: Password can't be empty")]
    [InlineData(ValidName, ValidEmail, "", "Validation failed: Password can't be empty")]
    [InlineData("", "", "", "Validation failed: Name can't be empty; Email can't be empty; Password can't be empty")]
    [InlineData(ValidName, ValidEmail, "221121", "Validation failed: Invalid Password! Password must contains: minimum 8 characters, at least one uppercase letter(A-Z), at least one lowercase letter(a-z), at least one number(0-9), at least one special character e.g. !@#$%^&*()")]
    [InlineData(ValidName, ValidEmail, "asfsa", "Validation failed: Invalid Password! Password must contains: minimum 8 characters, at least one uppercase letter(A-Z), at least one lowercase letter(a-z), at least one number(0-9), at least one special character e.g. !@#$%^&*()")]
    [InlineData(ValidName, ValidEmail, "534g34g43g34g34", "Validation failed: Invalid Password! Password must contains: minimum 8 characters, at least one uppercase letter(A-Z), at least one lowercase letter(a-z), at least one number(0-9), at least one special character e.g. !@#$%^&*()")]
    public async Task CreateUserAsync_ValidationFailed(string name, string email, string password, string expectedErrorMessage)
    {
        // Arrange
        var hashServiceMock = new Mock<IPasswordHashService>();
        var userNotificationService = new Mock<IEventService>();

        var user1 = new User { Name = "Alex", Email = "Alex@email.com", PasswordHash = "Gt9Yc4AiI", Role = UserRole.User };
        var user2 = new User { Name = "Jon", Email = "Jon@email.com", PasswordHash = "vmsC1QQbe2R", Role = UserRole.User };
        var user3 = new User { Name = "Sam", Email = "Sam@email.com", PasswordHash = "ZsCIqvoYlst2x", Role = UserRole.Admin };

        var options = new DbContextOptionsBuilder<HomeTaskDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        using var context = new HomeTaskDbContext(options);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        context.Users.AddRange(user1, user2, user3);

        await context.SaveChangesAsync();

        var userService = new UserService(hashServiceMock.Object, userNotificationService.Object, context);

        var createUserModel = new CreateUserRequestModel
        {
            Name = name,
            Email = email,
            Password = password,
            Role = UserRole.User
        };

        string resultErrorMessage = null;

        // Act
        try
        {
            _ = await userService.CreateUserAsync(createUserModel, default);
        }
        catch (Exception e)
        {
            resultErrorMessage = e.Message;
        }

        // Assert
        Assert.Equal(expectedErrorMessage, resultErrorMessage);
    }

    private void EqualUser(User expected, UserResponseModel result) 
    {
        Assert.Equal(expected.Id, result.Id);
        Assert.Equal(expected.Name, result.Name);
        Assert.Equal(expected.Role, result.Role);
    }
}