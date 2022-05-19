using Restaurant.Domain.Entities;

namespace Restaurant.Business.Responses
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string Role { get; set; }
        public byte[]? Image { get; set; }

        public UserResponse(User user)
        {
            Id = user.Id;
            Role = user.Role.ToString();
            Name = user.FirstName + " " + user.LastName;
            Email = user.Email;
            Image = user.Picture;
        }
    }
}
