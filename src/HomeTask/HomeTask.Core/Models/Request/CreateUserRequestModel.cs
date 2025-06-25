using HomeTask.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace HomeTask.Core.Models.Request;

public class CreateUserRequestModel : IValidatableObject
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }

    public void Validate()
    {
        var context = new ValidationContext(this);
        var results = new List<ValidationResult>();

        if (!Validator.TryValidateObject(this, context, results, true))
        {
            var errorMessages = string.Join("; ", results.Select(r => r.ErrorMessage));
            throw new ValidationException($"Validation failed: {errorMessages}");
        }

        //if (string.IsNullOrWhiteSpace(Name))
        //{
        //    throw new Exception($"{nameof(Name)} can't be empty");
        //}

        //if (string.IsNullOrWhiteSpace(Email))
        //{
        //    throw new Exception($"{nameof(Email)} can't be empty");
        //}

        //var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        //if (!emailRegex.IsMatch(Email))
        //{
        //    throw new Exception($"Invalid email");
        //}

        //if (string.IsNullOrWhiteSpace(Password))
        //{
        //    throw new Exception($"{nameof(Password)} can't be empty");
        //}

        //var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$");

        //if (!passwordRegex.IsMatch(Password))
        //{
        //    throw new Exception($"Invalid Password! Password must contains: minimum 8 characters, at least one uppercase letter(A-Z), at least one lowercase letter(a-z), at least one number(0-9), at least one special character e.g. !@#$%^&*()");
        //}
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            yield return new ValidationResult($"{nameof(Name)} can't be empty");
        }

        if (string.IsNullOrWhiteSpace(Email))
        {
            yield return new ValidationResult($"{nameof(Email)} can't be empty");
        }
        else 
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            if (!emailRegex.IsMatch(Email))
            {
                yield return new ValidationResult($"Invalid email");
            }
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            yield return new ValidationResult($"{nameof(Password)} can't be empty");
        }
        else 
        {
            var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$");

            if (!passwordRegex.IsMatch(Password))
            {
                yield return new ValidationResult($"Invalid Password! Password must contains: minimum 8 characters, at least one uppercase letter(A-Z), at least one lowercase letter(a-z), at least one number(0-9), at least one special character e.g. !@#$%^&*()");
            }
        }
    }
}