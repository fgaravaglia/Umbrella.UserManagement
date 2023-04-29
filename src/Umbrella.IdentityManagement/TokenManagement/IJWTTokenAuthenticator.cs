using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Umbrella.IdentityManagement.TokenManagement
{
    /// <summary>
    /// Abstraction of JWT token authentication
    /// </summary>
    public interface IJwtTokenAuthenticator
    {
        /// <summary>
        /// Autnethicates the client by basic auth, generating back the token
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>JWT Token</returns>
        string? Authenticate(string username, string password);
        /// <summary>
        /// VAlidates the token, extracting the user id. if token is not valid, the returned id is null
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        string? ValidateToken(string token);
    }
}