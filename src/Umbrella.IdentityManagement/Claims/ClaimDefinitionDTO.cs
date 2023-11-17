
using System.Diagnostics.CodeAnalysis;

namespace Umbrella.IdentityManagement.Claims
{
    /// <summary>
    /// class to model a single Claim
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ClaimDefinitionDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ClaimDefinitionDto()
        {
            Type = "";
            Value = "";
        }
    }
}