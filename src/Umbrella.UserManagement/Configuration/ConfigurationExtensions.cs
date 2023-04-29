using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Umbrella.UserManagement.Configuration
{
    /// <summary>
    /// Extensions to read userManagement section from appsettings
    /// </summary>
    public static class ConfigurationExtensions
    {
        internal const string SECTION_NAME = "UserManagement";

        /// <summary>
        /// Reads appsettings file
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static UserManagementSettings GetUserManagementSettings(this IConfiguration config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            var settings = new UserManagementSettings();
            config.GetSection(SECTION_NAME).Bind(settings);
            return settings;
        }

    }
}