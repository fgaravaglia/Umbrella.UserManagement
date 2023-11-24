using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Umbrella.UserManagement;

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
        public IEnumerable<Claim> Claims { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public UserDto? UserInformation { get; set; }
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
        /// <param name="claims"></param>
        /// <param name="user"></param>
        public TokenValidationResponse(IEnumerable<Claim> claims, UserDto user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.Name))
                throw new ArgumentNullException(nameof(user), "Name cannot be null");
            this.UserInformation = user;
            this.UserIdentifier = user.Name;

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
