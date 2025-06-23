namespace HomeTask.Core.Interfaces;

public interface IPasswordHashService
{
    string Hash(string password);
}
