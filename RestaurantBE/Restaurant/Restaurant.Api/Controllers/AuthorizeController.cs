using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Restaurant.Business.Configuration;
using Restaurant.Business.Requests;
using Restaurant.Business.Responses;
using Restaurant.Business.Services;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Entities.MessageResponses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Restaurant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly IAuthorizeService _authorizeService;

        public AuthorizeController(IAuthorizeService authorizeService)
        {
            _authorizeService = authorizeService;
        }

        [HttpPost]
        public async Task<IActionResult> AuthorizeAsync([FromBody] AuthorizeRequest request)
        {
            if (request.Email == null || request.Email == "")
            {
                return BadRequest(new AuthorizeErrorResponse(Messages.AuthenticationInvalidEmail));
            }
            if (request.Password == null || request.Password == "")
            {
                return BadRequest(new AuthorizeErrorResponse(Messages.AuthenticationInvalidPassword));
            }

            var response = await _authorizeService.Authorize(request);

            if (response == null)
            {
                return BadRequest(new AuthorizeErrorResponse(Messages.AuthenticationLoginError));
            }

            return Ok(response);
        }
    }
}
