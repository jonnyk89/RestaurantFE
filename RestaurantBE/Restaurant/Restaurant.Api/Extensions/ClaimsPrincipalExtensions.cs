using System.Security.Claims;

namespace Restaurant.Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal) => principal.FindFirstValue(ClaimTypes.NameIdentifier);

        public static string GetEmail(this ClaimsPrincipal principal) => principal.FindFirstValue(ClaimTypes.Email);

        public static string GetRole(this ClaimsPrincipal principal) => principal.FindFirstValue(ClaimTypes.Role);
    }
}
