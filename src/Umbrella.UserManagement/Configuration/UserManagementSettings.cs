using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Umbrella.UserManagement.Configuration
{
    public class UserManagementSettings
    {
        /// <summary>
        /// Persistence Type
        /// </summary>
        /// <value></value>
        public string PersistenceProvider {get; set;}

        public UserManagementSettings()
        {
            this.PersistenceProvider = "json";
        }
    }
}