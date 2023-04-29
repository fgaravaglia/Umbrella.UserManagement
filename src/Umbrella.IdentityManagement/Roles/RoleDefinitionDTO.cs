using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Umbrella.IdentityManagement.Roles
{
    /// <summary>
    /// class to model a single role
    /// </summary>
    public class RoleDefinitionDTO
    {
        /// <summary>
        /// Code to identify role
        /// </summary>
        /// <value></value>
        public string Role { get; set; }
        /// <summary>
        /// USer firendly text for current role
        /// </summary>
        /// <value></value>
        public string DisplayText { get; set; }
        /// <summary>
        /// CLaims related to current role
        /// </summary>
        /// <value></value>
        public List<ClaimDefinitionDTO> Claims { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public RoleDefinitionDTO()
        {
            this.Role = "USER";
            this.DisplayText = "";
            this.Claims = new List<ClaimDefinitionDTO>();
        }
    }
}