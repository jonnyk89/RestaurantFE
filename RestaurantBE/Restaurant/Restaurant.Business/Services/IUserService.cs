
using Restaurant.Business.Requests;
using Restaurant.Business.Responses;
using Restaurant.Domain.Entities;

namespace Restaurant.Business.Services
{
    public interface IUserService
    {
        Task<int> GetTotalUsersAsync();
        Task<List<User>> GetAllUsersAsync(int page, int pageSize);
        Task<User> GetUserByIdAsync(string userId);
        Task<MessageResponse> CreateUserAsync(UserCreateRequest request, string password);
        Task<MessageResponse> ClearUserPictureAsync(string userId);
        Task<MessageResponse> UpdateUserAsync(UserUpdateRequest request, string password, string userId);
        Task<MessageResponse> UpdateProfileAsync(UserUpdateProfileRequest request, string password, string userId);
        Task<MessageResponse> DeleteUserAsync(string currentUserId, string userId);
    }
}
