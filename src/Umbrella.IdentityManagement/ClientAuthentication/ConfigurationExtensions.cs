using Microsoft.Extensions.Configuration;

namespace Umbrella.IdentityManagement.ClientAuthentication
{
    /// <summary>
    /// Extensions to read authentication section from appsettings
    /// </summary>
    public static class ConfigurationExtensions
    {
        internal const string AUTHENTICATION_SECTION_NAME = "Authentication";

        /// <summary>
        /// Reads appsettings file
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static AuthenticationSettings GetAuthenticationSettings(this IConfiguration config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            var settings =   config.GetSection(AUTHENTICATION_SECTION_NAME).Get< AuthenticationSettings>();
            if(settings == null)
                throw new InvalidOperationException($"Wrong appSettings file: section '{ClientAuthentication.ConfigurationExtensions.AUTHENTICATION_SECTION_NAME}' is empty");
            return settings;
        }
        /// <summary>
        /// Gets the configured clients
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static IEnumerable<ClientSettings> GetClientSettings(this IConfiguration config)
        {
            var opt = config.GetSection(AUTHENTICATION_SECTION_NAME + ":Clients").Get<IEnumerable<ClientSettings>>();
            if (opt == null)
                throw new InvalidOperationException($"Wrong appSettings file: section '{AUTHENTICATION_SECTION_NAME}:Clients' is empty");
            if (!opt.Any())
                throw new InvalidOperationException($"Wrong appSettings file: section '{AUTHENTICATION_SECTION_NAME}:Clients' is empty");
            return opt;
        }

    }
}