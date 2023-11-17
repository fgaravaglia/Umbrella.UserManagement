using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbrella.IdentityManagement.Tests
{
    /// <summary>
    /// Manager to Create a Fake Instance of configuration
    /// </summary>
    internal static class ConfigurationManager
    {
        /// <summary>
        /// Initializes the configuration with in memory object
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static IConfiguration FromMemory(Dictionary<string, string> inMemorySettings)
        {
            if (inMemorySettings == null)
                throw new ArgumentNullException(nameof(inMemorySettings));
            //Dictionary<string, string> inMemorySettings = new Dictionary<string, string> {
            //        { "SecurityLogging:" +  nameof(SecurityLogSettings.ApplicationCode), settings.SecurityLog.ApplicationCode},
            //        { "SecurityLogging:" + nameof(SecurityLogSettings.Path), settings.SecurityLog.Path},
            //        { "SecurityLogging:" + nameof(SecurityLogSettings.BaseFileName), settings.SecurityLog.BaseFileName},
            //        { "SecurityLogging:" + nameof(SecurityLogSettings.MaxSizeLogFile), settings.SecurityLog.MaxSizeLogFile.ToString()},
            //        { "Databases:DiaDb:" +  nameof(DatabaseOptions.Type), settings.Databases["DiaDb"].Type.ToString()},
            //        { "Databases:DiaDb:" +  nameof(DatabaseOptions.Host), settings.Databases["DiaDb"].Host},
            //        { "Databases:DiaDb:" +  nameof(DatabaseOptions.Name), settings.Databases["DiaDb"].Name},
            //    };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }
    }
}
