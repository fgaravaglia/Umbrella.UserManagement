using System.Diagnostics.CodeAnalysis;
using Umbrella.IdentityManagement.Claims;

namespace Umbrella.IdentityManagement.Roles
{
    /// <summary>
    /// class to model a single role
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RoleDefinitionDto
    {
        /// <summary>
        /// Code to identify role
        /// </summary>
        /// <value></value>
        public string Role { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastUpdatedOn { get; set; }
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
            this.CreatedOn = DateTime.UtcNow;
            this.Claims = new List<ClaimDefinitionDto>();
        }
    }
}