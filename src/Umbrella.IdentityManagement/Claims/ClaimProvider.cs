using Microsoft.Extensions.Logging;
using System.Security.Claims;
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
                this._Logger.LogInformation("Retreiving user by name {Name}", name);
                var user = _UserRepository.GetByKey(name);
                if (user == null)
                {
                    // it is an application
                    this._Logger.LogWarning("User {Name} not found. Supposing it is an Application, but not registered?", name);
                    throw new IdentityValidationException($"User {name} not found. Supposing it is an Application, but not registered");
                }

                // read user and Roles
                var roles = GetRolesByUser(applicationId, user);

                // convert them in claims
                this._Logger.LogInformation("Generating claims...");
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Name));
                claims.Add(new Claim(ClaimTypes.Role, String.Join(";", user.Roles)));
                foreach (var role in roles)
                {
                    foreach (var claim in role.Claims)
                    {
                        if (!claims.Exists(x => x.Type == claim.Type))
                            claims.Add(new Claim(claim.Type, claim.Value));
                    }
                }
                this._Logger.LogInformation("Generated {Claims} claims", claims.Count);
            }
            return claims;
        }

        #region Private Methods

        IEnumerable<RoleDefinitionDto> GetRolesByUser(string applicationId, UserDto user)
        {
            var roles = new List<RoleDefinitionDto>();
            foreach (var r in user.Roles)
            {
                this._Logger.LogInformation("Retreiving role {Name}...", r);
                var dto = this._RoleRepositoryFactory.Build(applicationId).GetByKey(r);
                if (dto == null)
                {
                    this._Logger.LogWarning("Unable to find role {Role}", r);
                }
                else
                    roles.Add(dto);
            }
            this._Logger.LogInformation("Found {Roles} Roles", roles.Count);
            return roles;
        }

        #endregion
    }
}
