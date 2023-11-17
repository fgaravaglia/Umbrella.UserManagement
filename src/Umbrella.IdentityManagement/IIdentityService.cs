using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

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
        string AuthenticateClient(string clientId, string secret);
        /// <summary>
        /// Autnethicates the user by basic auth, generating back the token
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
        TokenValidationResponse ValidateToken(string token);
    }
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TokenValidationResponse
    {
        /// <summary>
        /// ID of user, if token is valid
        /// </summary>
        public string? UserIdentifier { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsValid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ErrorCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Constr for succesful validation
        /// </summary>
        public TokenValidationResponse(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException("userId");
            UserIdentifier = userId;
            IsValid = true;
            ErrorCode = "";
            Message = "";
        }
        /// <summary>
        /// Constr for failed validatoin
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public TokenValidationResponse(string code, string message)
        {
            IsValid = false;
            ErrorCode = code;
            Message = message;
        }
    }
}