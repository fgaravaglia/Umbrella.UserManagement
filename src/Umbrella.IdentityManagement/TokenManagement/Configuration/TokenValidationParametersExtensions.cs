using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbrella.IdentityManagement.TokenManagement.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public static class TokenValidationParametersExtensions
    {
        /// <summary>
        /// Generates the symmetric security key for JWT
        /// </summary>
        /// <param name="opts"></param>
        /// <returns></returns>
        public static SymmetricSecurityKey GenerateSecurityKey(this JwtSettings opts)
        {
            // converts the key in byte to crete proper symmetric key
            var keyBytes = Encoding.UTF8.GetBytes(opts.SigningKey);
            return new SymmetricSecurityKey(keyBytes);
        }
        /// <summary>
        /// Gets the validation parameteres
        /// </summary>
        /// <param name="opts"></param>
        /// <returns></returns>
        public static TokenValidationParameters ToValidationParameters(this JwtSettings opts)
        {
            // set validation parameters
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = opts.ValidateIssuer,
                ValidIssuer = opts.ValidIssuer,
                ValidateAudience = opts.ValidateAudience,
                ValidAudience = opts.ValidAudience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKeys = new List<SecurityKey>() { opts.GenerateSecurityKey() },
                ValidateLifetime = true,
            };
            return validationParameters;
        }
    }
}
