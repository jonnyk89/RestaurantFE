using Restaurant.Domain.Entities;

namespace Restaurant.Data.Contracts
{
    public interface IUserRepository
    {
        Task<int> GetTotalUsersAsync();
        Task<List<User>> GetAllUsersAsync(int page, int pageSize);
        Task<User> GetUserByIdAsync(string userId);
        Task<User> GetUserByEmailAsync(string email);
        Task CreateUserAsync(User user, string password);
        Task ClearUserPictureAsync(User user);
        Task UpdateUserAsync(User user, string password);
        Task DeleteUserAsync(User user);
    }
}
