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
        /// Id of application for which the repo is providing the roles
        /// </summary>
        string ApplicationId { get; }

        /// <summary>
        /// GEts all avaialble roles
        /// </summary>
        /// <returns></returns>
        IEnumerable<RoleDefinitionDto> GetAll();
        /// <summary>
        /// gets the specific role
        /// </summary>
        /// <param name="key">role code</param>
        /// <returns></returns>
        RoleDefinitionDto? GetByKey(string key);
        /// <summary>s
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