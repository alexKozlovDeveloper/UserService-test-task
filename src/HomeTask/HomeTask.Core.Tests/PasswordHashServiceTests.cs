using HomeTask.Core.Interfaces.Implementations;

namespace HomeTask.Core.Tests;
public class PasswordHashServiceTests
{
    [Fact]
    public void Hash_ReturnPasswordHash()
    {
        // Arrange
        var password = "avcx@#241FD";

        var passwordHashService = new PasswordHashService();        

        // Act
        var resultPasswordHash = passwordHashService.Hash(password);

        // Assert
        Assert.True(passwordHashService.VerifyPassword(resultPasswordHash, password));
        Assert.False(passwordHashService.VerifyPassword(resultPasswordHash, "wrongPassword"));
    }
}