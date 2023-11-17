using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Umbrella.IdentityManagement.Claims
{
    /// <summary>
    /// Abstraction to retreive the claims for given user
    /// </summary>
    public interface IClaimProvider
    {
        /// <summary>
        /// gets the claims
        /// </summary>
        /// <param name="name"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        IEnumerable<Claim> GetByIdentityName(string name, string applicationId);
    }
}
