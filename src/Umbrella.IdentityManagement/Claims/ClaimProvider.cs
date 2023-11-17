using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Umbrella.IdentityManagement.ErrorManagement;
using Umbrella.IdentityManagement.Roles;
using Umbrella.UserManagement;

namespace Umbrella.IdentityManagement.Claims
{
    /// <summary>
    /// Concrete implementation of claims provider
    /// </summary>
    public class ClaimProvider : IClaimProvider
    {
        #region Fields
        readonly ILogger _Logger;
        readonly IUserRepository _UserRepository;
        readonly IRoleRepositoryFactory _RoleRepositoryFactory;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="userRepository"></param>
        /// <param name="factory"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ClaimProvider(ILogger logger, IUserRepository userRepository, IRoleRepositoryFactory factory)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _UserRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _RoleRepositoryFactory = factory ?? throw new ArgumentNullException(nameof(factory));
        }
        /// <summary>
        /// <inheritdoc cref="IClaimProvider.GetByIdentityName(string, string)"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IEnumerable<Claim> GetByIdentityName(string name, string applicationId)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(applicationId))
                throw new ArgumentNullException(nameof(applicationId));
            List<Claim> claims = new();

            //check if it is a user
            using (this._Logger.BeginScope(new Dictionary<string, string>()
            {
                ["Method"] = nameof(GetByIdentityName)
            }))
            {
                this._Logger.LogInformation("Retreiving user by name {name}", name);
                var user = _UserRepository.GetByKey(name);
                if (user == null)
                {
                    // it is an application
                    this._Logger.LogWarning("User {name} not found. Supposing it is an Application, but not registered?", name);
                    throw new IdentityValidationException($"User {name} not found. Supposing it is an Application, but not registered");
                }
                else
                {
                    // read user and Roles
                    var roles = new List<RoleDefinitionDto>();
                    foreach (var r in user.Roles)
                    {
                        this._Logger.LogInformation("Retreiving role {name}...", r);
                        var dto = this._RoleRepositoryFactory.Build(applicationId).GetByKey(r);
                        if (dto == null)
                        {
                            this._Logger.LogWarning("Unable to find role {role}", r);
                        }
                        else
                            roles.Add(dto);
                    }
                    this._Logger.LogInformation("Found {roles} Roles", roles.Count);

                    // convert them in claims
                    this._Logger.LogInformation("Generating claims...");
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Name));
                    foreach (var role in roles)
                    {
                        foreach (var claim in role.Claims)
                        {
                            if (!claims.Any(x => x.Type == claim.Type))
                                claims.Add(new Claim(claim.Type, claim.Value));
                        }
                    }
                    this._Logger.LogInformation("Generated {claims} claims", claims.Count);
                }
            }


            return claims;
        }
    }
}
