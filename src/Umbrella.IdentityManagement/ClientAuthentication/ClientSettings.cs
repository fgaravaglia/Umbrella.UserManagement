using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Umbrella.IdentityManagement.ClientAuthentication
{
    /// <summary>
    /// Settings to store couple ClientID - AppId
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ClientSettings
    {
        /// <summary>
        /// Name of client
        /// </summary>
        /// <value></value>
        public string Name { get; set; }
        /// <summary>
        /// ID of client
        /// </summary>
        /// <value></value>
        public string ClientID { get; set; }
        /// <summary>
        /// ID of application
        /// </summary>
        /// <value></value>
        public string ApplicationID { get; set; }
        /// <summary>
        /// secret
        /// </summary>
        /// <value></value>
        public string SecretID { get; set; }

        /// <summary>
        /// empty COnstr
        /// </summary>
        public ClientSettings()
        {
            this.Name = "";
            this.ApplicationID = "";
            this.ClientID = "";
            this.SecretID = "";
        }
    }
}