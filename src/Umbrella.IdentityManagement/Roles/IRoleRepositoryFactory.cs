using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbrella.IdentityManagement.Roles
{
    /// <summary>
    /// factory to instance the components
    /// </summary>
    public interface IRoleRepositoryFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        IRoleRepository Build(string applicationId);
    }
    /// <summary>
    /// Concrete implementation of factory
    /// </summary>
    internal class RoleRepositoryFactory : IRoleRepositoryFactory
    {
        #region Fields
        readonly IEnumerable<IRoleRepository> _Repositories;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repositories"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public RoleRepositoryFactory(IEnumerable<IRoleRepository> repositories)
        {
            if (repositories == null) throw new ArgumentNullException(nameof(repositories));
            if (!repositories.Any()) throw new ArgumentNullException(nameof(repositories));
            var list = new List<IRoleRepository>();
            list.AddRange(repositories);
            this._Repositories = list;
        }
        /// <summary>
        /// resolves the repo
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public IRoleRepository Build(string applicationId)
        {
            if (String.IsNullOrEmpty(applicationId)) throw new ArgumentNullException(nameof(applicationId));
            var repos = this._Repositories.Where(x => x.ApplicationId.Equals(applicationId, StringComparison.InvariantCultureIgnoreCase));
            if (!repos.Any())
                throw new InvalidOperationException("Wrong Dependency configuration: no repository found for application " + applicationId);
            if (repos.Count() > 1)
                throw new InvalidOperationException("Wrong Dependency configuration: found more than on repository for application " + applicationId);

            return repos.First();
        }
    }
}
