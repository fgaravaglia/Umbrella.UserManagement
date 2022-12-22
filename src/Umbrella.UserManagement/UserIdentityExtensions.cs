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
            if (principal.Identity == null)
                throw new ArgumentNullException(nameof(principal));

            if (!principal.Identity.IsAuthenticated)
                throw new InvalidOperationException($"User must to be logged!");

            if (principal.HasClaim(x => x.Type == "picture"))
                return principal.Claims.Single(x => x.Type == "picture").Value;

            return "https://www.tutorialrepublic.com/examples/images/avatar/1.jpg";
        }

    }
}