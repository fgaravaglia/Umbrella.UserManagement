using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Umbrella.IdentityManagement.Roles
{
    /// <summary>
    /// class to model a single role
    /// </summary>
    public class RoleDefinitionDto
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
        public List<ClaimDefinitionDto> Claims { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public RoleDefinitionDto()
        {
            this.Role = "USER";
            this.DisplayText = "";
            this.Claims = new List<ClaimDefinitionDto>();
        }
    }
}