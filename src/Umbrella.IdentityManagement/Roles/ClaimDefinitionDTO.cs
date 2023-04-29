using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Umbrella.IdentityManagement.Roles
{
    /// <summary>
    /// class to model a single Claim
    /// </summary>
    public class ClaimDefinitionDTO
    {
        public string Type { get; set; }

        public string Value { get; set; }

        public ClaimDefinitionDTO()
        {
            this.Type = "";
            this.Value = "";
        }
    }
}