using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Umbrella.IdentityManagement.ClientAuthentication
{
    /// <summary>
    /// Section Authetication from appSettings file
    /// </summary>
    public class AuthenticationSettings
    {
        /// <summary>
        /// List of IDs for enabled applications
        /// </summary>
        /// <value></value>
        public List<ClientSettings> Clients { get; set; }

        /// <summary>
        /// EMpty COnstr
        /// </summary>
        public AuthenticationSettings()
        {
            this.Clients = new List<ClientSettings>();
        }
    }
    
}