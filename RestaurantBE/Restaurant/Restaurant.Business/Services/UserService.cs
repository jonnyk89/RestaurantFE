
using Microsoft.AspNetCore.Http;
using Restaurant.Business.Requests;
using Restaurant.Business.Responses;
using Restaurant.Data.Contracts;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Entities.MessageResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int> GetTotalUsersAsync()
        {
            return await _userRepository.GetTotalUsersAsync();
        }

        public async Task<List<User>> GetAllUsersAsync(int page, int pageSize)
        {
            var allUsers = await _userRepository.GetAllUsersAsync(page, pageSize);
            return allUsers;
        }
        public async Task<User> GetUserByIdAsync(string userId)
        {
            var result = await _userRepository.GetUserByIdAsync(userId);
            return result;
        }
        public async Task<MessageResponse> CreateUserAsync(UserCreateRequest request, string password)
        {
            var targetUser = await _userRepository.GetUserByEmailAsync(request.Email);
            if (targetUser != null)
            {
                return new MessageResponse(Messages.UserEmailTaken);
            }

            User user = new User()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Role = request.Role,
                Picture = null,
                UserName = request.FirstName,
            };

            await _userRepository.CreateUserAsync(user, password);
            return new MessageResponse(Messages.UserCreated, user.Id);
        }

        public async Task<MessageResponse> ClearUserPictureAsync(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            user.Picture = null;
            await _userRepository.ClearUserPictureAsync(user);
            return new MessageResponse(Messages.UserPictureCleared);
        }

        public async Task<MessageResponse> UpdateUserAsync(UserUpdateRequest request, string password, string userId)
        {
            var targetUser = await _userRepository.GetUserByEmailAsync(request.Email);
            var existingUser = await _userRepository.GetUserByIdAsync(userId);

            if (existingUser == null)
            {
                return new MessageResponse(Messages.UserNotFound);
            }

            if (request.Email != null && targetUser != null && targetUser.Email == request.Email)
            {
                return new MessageResponse(Messages.UserEmailTaken);
            }

            existingUser.FirstName = request.FirstName ?? existingUser.FirstName;
            existingUser.LastName = request.LastName ?? existingUser.LastName;
            existingUser.UserName = request.FirstName;
            existingUser.Email = request.Email ?? existingUser.Email;
            existingUser.Role = request.Role;

            await _userRepository.UpdateUserAsync(existingUser, password);
            return new MessageResponse(Messages.UserUpdated, existingUser.Id);
        }

        public async Task<MessageResponse> UpdateProfileAsync(UserUpdateProfileRequest request, string password, string userId)
        {
            var targetUser = await _userRepository.GetUserByEmailAsync(request.email);
            if (request.picture != null)
            {
                var file = request.picture;
                //Getting FileName
                var fileName = Path.GetFileName(file.FileName);
                //Getting file Extension
                var fileExtension = Path.GetExtension(fileName);

                if (((file.Length / 1024f) / 1024f) > 2 || (fileExtension != ".png" && fileExtension != ".jpg"))
                {
                    return new MessageResponse(Messages.UserImageInvalid);
                }
            }

            if (request.email != null && targetUser != null && targetUser.Email == request.email)
            {
                return new MessageResponse(Messages.UserEmailTaken);
            }

            var existingUser = await _userRepository.GetUserByIdAsync(userId);

            existingUser.FirstName = request.firstName ?? existingUser.FirstName;
            existingUser.LastName = request.lastName ?? existingUser.LastName;
            existingUser.UserName = request.firstName ?? existingUser.FirstName;
            existingUser.Email = request.email ?? existingUser.Email;
            if (request.picture != null)
            {
                existingUser.Picture = FileToByteArray(request.picture);
            }

            await _userRepository.UpdateUserAsync(existingUser, password);
            return new MessageResponse(Messages.UserUpdated, existingUser.Id);
        }

        public async Task<MessageResponse> DeleteUserAsync(string currentUserId, string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return new MessageResponse(Messages.UserNotFound);
            }

            if (currentUserId == userId)
            {
                return new MessageResponse(Messages.UserAdminCannotDeleteSelf);
            }

            await _userRepository.DeleteUserAsync(user);
            return new MessageResponse(Messages.UserDeleted, userId);
        }

        private byte[] FileToByteArray(IFormFile file)
        {
            var result = new byte[1024];

            using (var target = new MemoryStream())
            {
                file.CopyTo(target);
                result = target.ToArray();
            }

            return result;
        }
    }
}
