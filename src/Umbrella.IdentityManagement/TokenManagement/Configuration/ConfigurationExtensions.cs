using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbrella.IdentityManagement.TokenManagement.Configuration;
using Microsoft.Extensions.Configuration;

namespace Umbrella.IdentityManagement.TokenManagement.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// REads appSettings.json file
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static JwtSettings GetJwtSettings(this IConfiguration config)
        {
            if(config == null)
                throw new ArgumentNullException(nameof(config));
            // read the configuration
            JwtSettings? settings =  config.GetSection(ClientAuthentication.ConfigurationExtensions.AUTHENTICATION_SECTION_NAME + ":Jwt").Get<JwtSettings>();
            if (settings == null)
                throw new InvalidOperationException($"Wrong appSettings file: section '{ClientAuthentication.ConfigurationExtensions.AUTHENTICATION_SECTION_NAME}:JwtOptions' is empty");
            return settings;
        }

    }
}