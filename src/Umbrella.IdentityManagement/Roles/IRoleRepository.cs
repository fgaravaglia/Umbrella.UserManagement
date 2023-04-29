using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Umbrella.IdentityManagement.Roles
{
    /// <summary>
    /// Abstarction for Storage of Role definitions
    /// </summary>
    public interface IRoleRepository
    {
        /// <summary>
        /// GEts all avaialble roles
        /// </summary>
        /// <returns></returns>
        IEnumerable<RoleDefinitionDto> GetAll();
        /// <summary>
        /// gets the specific role
        /// </summary>
        /// <param name="role">role code</param>
        /// <returns></returns>
        RoleDefinitionDto? GetByKey(string role);
        /// <summary>
        /// saves the role
        /// </summary>
        /// <param name="role"></param>
        void Save(RoleDefinitionDto role);
        /// <summary>
        /// Regenrates the list of roles. USeful for startup
        /// </summary>
        void InitializeRoles();
    }
}