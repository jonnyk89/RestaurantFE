using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Business.Requests;
using Restaurant.Business.Responses;
using Restaurant.Business.Services;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Entities.MessageResponses;
using System.Security.Claims;

namespace Restaurant.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("total"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTotalUsersAsync()
        {
            int total = await _userService.GetTotalUsersAsync();
            var result = new TotalResponse(total);
            return Ok(result);
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsersAsync([FromQuery] int page, [FromQuery] int pageSize)
        {
            List<User> Users = await _userService.GetAllUsersAsync(page, pageSize);
            List<UserResponse> Responses = new List<UserResponse>();
            foreach (User user in Users)
            {
                Responses.Add(new UserResponse(user));
            }
            return Ok(Responses);
        }

        [HttpGet("{userId}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserByIdAsync([FromRoute] string userId)
        {
            User user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound(Messages.UserNotFound);
            }
            UserResponse response = new UserResponse(user);

            return Ok(response);
        }

        [HttpGet("me"), Authorize(Roles = "Admin,Waiter")]
        public async Task<IActionResult> GetCurrentUserAsync()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (currentUser == null)
            {
                return NotFound(Messages.UserNotFound);
            }
            User user = await _userService.GetUserByIdAsync(currentUserID);
            UserResponse response = new UserResponse(user);

            return Ok(response);
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUserAsync(UserCreateRequest request)
        {
            var response = await _userService.CreateUserAsync(request, request.Password);
            return Ok(response);
        }

        [HttpPut("me/clearPicture"), Authorize(Roles = "Admin,Waiter")]
        public async Task<IActionResult> ClearUserPictureAsync()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (currentUser == null)
            {
                return NotFound(Messages.UserNotFound);
            }
            var response = await _userService.ClearUserPictureAsync(currentUserID);
            return Ok(response);
        }

        [HttpPut("{userId}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserAsync([FromRoute]string userId, UserUpdateRequest request)
        {
            var response = await _userService.UpdateUserAsync(request, request.Password, userId);

            return Ok(response);
        }

        [HttpPut("me"), Authorize(Roles = "Admin,Waiter")]
        public async Task<IActionResult> UpdateCurrentUserAsync([FromForm]UserUpdateProfileRequest request)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (currentUser == null)
            {
                return NotFound(Messages.UserNotFound);
            }

            var response = await _userService.UpdateProfileAsync(request, request.password, currentUserID);

            return Ok(response);
        }

        [HttpDelete("{userId}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] string userId)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            var response = await _userService.DeleteUserAsync(currentUserID, userId);

            return Ok(response);
        }
    }
}
