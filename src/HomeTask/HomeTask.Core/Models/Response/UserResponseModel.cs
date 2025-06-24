using HomeTask.Core.Entities;

namespace HomeTask.Core.Models.Response;

public class UserResponseModel
{
    public string Name { get; set; }
    public UserRole Role { get; set; }
}
