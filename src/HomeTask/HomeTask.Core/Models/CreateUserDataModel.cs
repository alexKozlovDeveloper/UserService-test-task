using System.Text.RegularExpressions;

namespace HomeTask.Core.Models;

public class CreateUserDataModel
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name)) 
        {
            throw new Exception($"{nameof(Name)} can't be empty");
        }

        if (string.IsNullOrWhiteSpace(Email))
        {
            throw new Exception($"{nameof(Name)} can't be empty");
        }

        var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        if (!emailRegex.IsMatch(Email))
        {
            throw new Exception($"Invalid email");
        }

        var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$");

        if (!passwordRegex.IsMatch(Password))
        {
            throw new Exception($"Invalid Password! Password must contains: minimum 8 characters, at least one uppercase letter(A-Z), at least one lowercase letter(a-z), at least one number(0-9), at least one special character e.g. !@#$%^&*()");
        }
    }
}