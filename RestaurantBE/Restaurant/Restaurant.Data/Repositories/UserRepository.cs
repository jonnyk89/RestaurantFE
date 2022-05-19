using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Contracts;
using Restaurant.Data.Paging;
using Restaurant.Domain.Entities;

namespace Restaurant.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;
        private UserManager<User> _userManager;

        public UserRepository(UserManager<User> userManager, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        

        public async Task<int> GetTotalUsersAsync()
        {
            int total = await _appDbContext.Users.CountAsync();
            return total;
        }

        public async Task<List<User>> GetAllUsersAsync(int page, int pageSize)
        {
            var result = _userManager.Users;
            if (page != 0 && pageSize != 0)
            {
                result = new Paginator<User>().Paginate(result, page, pageSize);
            }
            return await result.ToListAsync();
        }
        public async Task<User> GetUserByIdAsync(string userId)
            => await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task CreateUserAsync(User user, string password)
        {
            await _userManager.CreateAsync(user, password);
            await _userManager.AddToRoleAsync(user, user.Role.ToString());
            await _appDbContext.SaveChangesAsync();
        }
        public async Task ClearUserPictureAsync(User user)
        {
            await _userManager.UpdateAsync(user);
            await _appDbContext.SaveChangesAsync();
        }
        public async Task UpdateUserAsync(User user, string password)
        {
            if (password != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                await _userManager.ResetPasswordAsync(user, token, password);
            }

            await _userManager.UpdateAsync(user);
            await _appDbContext.SaveChangesAsync();
            
        }
        public async Task DeleteUserAsync(User user)
        {
            await _userManager.DeleteAsync(user);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
