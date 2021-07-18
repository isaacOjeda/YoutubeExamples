using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace MyFirstApi.Helpers
{
    public static class AppExtensions
    {
        public static string GetUserEmail(this ClaimsPrincipal user)
        {
            var emailClaim = user.Claims.FirstOrDefault(q => q.Type == ClaimTypes.Email);

            if (emailClaim is null)
            {
                return null;
            }

            return emailClaim.Value;
        }

        public static string GetUserId(this ClaimsPrincipal user)
        {
            var userId = user.Claims.FirstOrDefault(q => q.Type == ClaimTypes.Sid);

            if (userId is null)
            {
                return null;
            }

            return userId.Value;
        }
    }
}