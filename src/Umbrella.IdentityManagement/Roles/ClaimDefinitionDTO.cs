using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Umbrella.IdentityManagement.Roles
{
    /// <summary>
    /// class to model a single Claim
    /// </summary>
    public class ClaimDefinitionDto
    {
        public string Type { get; set; }

        public string Value { get; set; }

        public ClaimDefinitionDto()
        {
            this.Type = "";
            this.Value = "";
        }
    }
}