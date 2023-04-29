using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbrella.IdentityManagement.TokenManagement.Configuration;
using Microsoft.Extensions.Configuration;

namespace Umbrella.IdentityManagement.TokenManagement.Configuration
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// REads appSettings.json file
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static JwtSettings ReadJwtSettings(this IConfiguration config)
        {
            if(config == null)
                throw new ArgumentNullException(nameof(config));
            // read the configuration
            JwtSettings settings = new JwtSettings();
            var section = config.GetSection(ClientAuthentication.ConfigurationExtensions.SECTION_NAME + ".JWT");
            if (section == null)
                throw new InvalidOperationException($"Wrong Configuration: section JWT is missing");
            // read and validate settings
            section.Bind(settings);
            return settings;
        }

    }
}