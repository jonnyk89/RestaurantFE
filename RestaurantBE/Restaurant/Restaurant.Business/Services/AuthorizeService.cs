using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Restaurant.Business.Configuration;
using Restaurant.Business.Requests;
using Restaurant.Business.Responses;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Business.Services
{
    public class AuthorizeService : IAuthorizeService
    {
        private readonly AuthOptions _options;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AuthorizeService(
            AuthOptions options,
            SignInManager<User> signInManager,
            UserManager<User> userManager)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<AuthorizeResponse> Authorize(AuthorizeRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return null;
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
            if (!signInResult.Succeeded)
            {
                return null;
            }

            var response = new AuthorizeResponse
            {
                AccessToken = await GetAccessTokenAsync(user),
                TokenType = "Bearer",
                ExpiresIn = _options.TokenLifetimeSeconds
            };

            return response;
        }

        private async Task<string> GetAccessTokenAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var signingKey = new SymmetricSecurityKey(_options.SecurityKeyAsBytes);
            var roles = await _userManager.GetRolesAsync(user);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(GetClaims(user, roles)),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature),
                Issuer = _options.Issuer,
                Audience = _options.Audience,
                Expires = DateTime.UtcNow.AddSeconds(_options.TokenLifetimeSeconds)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }

        private static IEnumerable<Claim> GetClaims(User user, IEnumerable<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
            };

            if (roles.Count() != 0)
            {
                claims.Add(new Claim("role", roles.FirstOrDefault()));
            }

            return claims;
        }
    }
}
