using HomeTask.Core.Entities;

namespace HomeTask.Core.Models.Response;

public record UserResponseModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public UserRole Role { get; set; }
}
