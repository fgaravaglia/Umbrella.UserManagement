using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Security.Claims;

[assembly: InternalsVisibleTo("Umbrella.IdentityManagement.Tests")]

namespace Umbrella.IdentityManagement
{
    /// <summary>
    /// Abstraction of authentication
    /// </summary>
    public interface IIdentityService
    {
        /// <summary>
        /// it tries to authenticate cliant application. if successfull, you get the token in response
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        AuthenticationResponse AuthenticateClient(string clientId, string secret);
        /// <summary>
        /// Autnethicates the user by basic auth, generating back the token
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>JWT Token</returns>
        AuthenticationResponse Authenticate(string username, string password);
        /// <summary>
        /// VAlidates the token, extracting the user id. if token is not valid, the returned id is null
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        TokenValidationResponse ValidateToken(string token);
    }
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AuthenticationResponse
    { 
        /// <summary>
        /// Authentication Token
        /// </summary>
        public string? Token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ClaimsIdentity? Identity { get; set; }
        /// <summary>
        /// True if authentication is succesfull
        /// </summary>
        public bool IsAuthenticated { get { return this.Identity != null; } }
    }
}