using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Umbrella.IdentityManagement.TokenManagement.Configuration
{
    /// <summary>
    /// DTO to map appSettings.json section
    ///  "JWT": {
    /// "ValidAudience": "http://localhost:4200",
    /// "ValidIssuer": "http://localhost:5000",
    /// "Secret": "JWTAuthenticationHIGHsecuredPasswordVVVp1OH7Xzyr"
    /// }
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class JwtSettings
    {
        /// <summary>
        /// List of audience, separated by ;
        /// </summary>
        /// <value></value>
        public string ValidAudience { get; set; }
        /// <summary>
        /// Issues, separated by ;
        /// </summary>
        /// <value></value>
        public string ValidIssuer { get; set; }
        /// <summary>
        /// Secret used to generate token and sign in
        /// </summary>
        /// <value></value>
        public string SigningKey { get; set; }
        /// <summary>
        /// Duration in Minutes of generated token
        /// </summary>
        /// <value></value>
        public int TokenValidityInMinutes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public bool ValidateIssuer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public bool ValidateAudience { get; set; }

        /// <summary>
        /// Default COnstr
        /// </summary>
        public JwtSettings()
        {
            this.ValidAudience = "";
            this.ValidIssuer = "";
            this.SigningKey = "";
            this.TokenValidityInMinutes = 15;
            this.ValidateIssuer = true;
            this.ValidateAudience = true;
        }
    }
}