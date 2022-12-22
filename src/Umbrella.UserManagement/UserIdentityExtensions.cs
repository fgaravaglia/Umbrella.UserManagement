using System;
using System.Linq;
using System.Security.Claims;

namespace Umbrella.UserManagement
{
    /// <summary>
    /// Helper for identity
    /// </summary>
    public static class UserIdentityExtensions
    {
        /// <summary>
        /// Gets the url of image for the logged user
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string GetUserImageUrl(this ClaimsPrincipal principal)
        {
            if(principal.Identity ==null)
                throw new ArgumentNullException(nameof(principal));
            
            if(!principal.Identity.IsAuthenticated)
                throw new InvalidOperationException($"User must to be logged!");
            
            if(principal.HasClaim(x => x.Type =="picture"))
                return principal.Claims.Single(x => x.Type == "picture").Value;

            return "https://www.tutorialrepublic.com/examples/images/avatar/1.jpg";
        }

        /// <summary>
        /// Checks if user is ADmin or not, based on his roles
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static bool IsAdministrator(this ClaimsPrincipal principal)
        {
             if(principal.Identity ==null)
             return false;

               if(!principal.Identity.IsAuthenticated)
               return false;
            
             var roleClaims = principal.Claims.Where(x => x.Type == ClaimTypes.Role).ToList();
             if(roleClaims.Count(x => x.Value == "ADMIN") > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Checks if user is ADmin or not, based on his roles
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static bool HasRoleOf(this ClaimsPrincipal principal, string role)
        {
            if(String.IsNullOrEmpty(role))
                return false;

            if (principal.Identity == null)
                return false;

            if (!principal.Identity.IsAuthenticated)
                return false;

            var roleClaims = principal.Claims.Where(x => x.Type == ClaimTypes.Role).ToList();
            if (roleClaims.Count(x => x.Value == role) > 0)
                return true;
                
            return false;
        }
    }
}