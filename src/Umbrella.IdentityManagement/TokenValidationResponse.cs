using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Umbrella.IdentityManagement
{
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
/// <value></value>
        public IEnumerable<Claim> Claims{get; set;}
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
        /// <param name="userId"></param>
        /// <param name="claims"></param>
        public TokenValidationResponse(string userId, IEnumerable<Claim> claims)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));
            UserIdentifier = userId;
            IsValid = true;
            ErrorCode = "";
            Message = "";
            var newListOfCLaims = new List<Claim>();
            newListOfCLaims.AddRange(claims);
            this.Claims = newListOfCLaims;
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
            Claims = new List<Claim>();
        }
    }
}
