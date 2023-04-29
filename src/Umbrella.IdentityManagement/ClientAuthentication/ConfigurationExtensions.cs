using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Umbrella.IdentityManagement.ClientAuthentication
{
    /// <summary>
    /// Extensions to read authentication section from appsettings
    /// </summary>
    public static class ConfigurationExtensions
    {
        internal const string SECTION_NAME = "Authentication";

        /// <summary>
        /// Reads appsettings file
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static AuthenticationSettings GetAuthenticationSettings(this IConfiguration config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            var settings = new AuthenticationSettings();
            config.GetSection(SECTION_NAME).Bind(settings);
            return settings;
        }

    }
}