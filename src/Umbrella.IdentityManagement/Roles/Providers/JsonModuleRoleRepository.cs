using System.Text.Json;
using Umbrella.IdentityManagement.Claims;

namespace Umbrella.IdentityManagement.Roles.Providers
{
    /// <summary>
    /// IMplementation of repository based on json file
    /// </summary>
    public class JsonModuleRoleRepository : IRoleRepository
    {
        #region Attributes
        readonly string _Path;
        readonly string _FileName;
        readonly object _Locker = new object();
        readonly IEnumerable<IModuleClaimProvider> _ClaimsProviders;
        readonly JsonSerializerOptions _SerializerOptions;
        #endregion

        /// <summary>
        /// Id of application for which the repo is providing the roles
        /// </summary>
        public string ApplicationId { get; private set; }


        /// <summary>
        /// /
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        public JsonModuleRoleRepository(string path, string applicationId, IEnumerable<IModuleClaimProvider> claimsProviders)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            this._Path = path;

            if (string.IsNullOrEmpty(applicationId))
                throw new ArgumentNullException(nameof(applicationId));
            this.ApplicationId = applicationId;
            _FileName = $"{this.ApplicationId}-Roles.json";
            this._SerializerOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            this._ClaimsProviders = claimsProviders ?? new List<IModuleClaimProvider>();
        }
        /// <summary>
        /// <inheritdoc cref="IRoleRepository.GetAll"/>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleDefinitionDto> GetAll()
        {
            var jsonString = "";
            if (!File.Exists(Path.Combine(_Path, _FileName)))
                return new List<RoleDefinitionDto>();

            //persist data
            lock (_Locker)
            {
                jsonString = File.ReadAllText(Path.Combine(_Path, _FileName));
            }
            return JsonSerializer.Deserialize<List<RoleDefinitionDto>>(jsonString, this._SerializerOptions) ?? new List<RoleDefinitionDto>();
        }
        /// <summary>
        /// <inheritdoc cref="IRoleRepository.GetByKey(string)"/>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public RoleDefinitionDto? GetByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            return GetAll().SingleOrDefault(x => x.Role.Equals(key, StringComparison.InvariantCultureIgnoreCase));
        }
        /// <summary>
        /// <inheritdoc cref="IRoleRepository.Save(RoleDefinitionDto)"/>
        /// </summary>
        /// <param name="role"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Save(RoleDefinitionDto role)
        {
            if (role is null)
                throw new ArgumentNullException(nameof(role));
            if (string.IsNullOrEmpty(role.Role))
                throw new ArgumentNullException(nameof(role));

            var roles = GetAll().ToList();

            var existingItem = roles.SingleOrDefault(x => x.Role == role.Role);
            if (existingItem == null)
                roles.Add(role);
            else
            {
                existingItem.DisplayText = role.DisplayText;
                existingItem.Claims.Clear();
                existingItem.Claims.AddRange(role.Claims);
            }
        }

        /// <summary>
        /// Regenrates the list of roles. USeful for startup
        /// </summary>
        public virtual void InitializeRoles()
        {
            var list = new List<RoleDefinitionDto>();
            list.Add(new RoleDefinitionDto()
            {
                Role = "ADMIN",
                DisplayText = "System Administrator",
                Claims = new List<ClaimDefinitionDto>()
                {
                    new ClaimDefinitionDto(){Type="https://umbrella/adminTools", Value="W" },
                }
            });

            // add new claims for existing roles
            foreach (var provider in _ClaimsProviders)
            {
                var claimsPerRoles = provider.GetClaimsPerRole();
                foreach (var role in claimsPerRoles)
                {
                    var existingRole = list.SingleOrDefault(x => x.Role.ToLowerInvariant() == role.Key.ToUpperInvariant());
                    if (existingRole == null)
                        continue;

                    foreach (var claim in role.Value)
                    {
                        if (!existingRole.Claims.Exists(x => x.Type == claim.Type))
                            existingRole.Claims.Add(claim);
                    }
                }
            }
            SaveListOfRoles(list);
        }

        #region Protected Methods

        protected void SaveListOfRoles(List<RoleDefinitionDto> roles)
        {
            //serialize li
            string jsonString = JsonSerializer.Serialize(roles, this._SerializerOptions);

            //persist data
            lock (_Locker)
            {
                File.WriteAllText(Path.Combine(_Path, _FileName), jsonString);
            }

        }

        #endregion


    }
}