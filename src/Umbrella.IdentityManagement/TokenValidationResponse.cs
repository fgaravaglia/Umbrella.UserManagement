using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
                throw new ArgumentNullException(nameof(userId));
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
