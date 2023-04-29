using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Umbrella.IdentityManagement.Roles
{
    /// <summary>
    /// IMplementation of repository based on json file
    /// </summary>
    public class JsonRoleRepository : IRoleRepository
    {
        #region Attributes
        readonly string _Path;
        readonly string _FileName;
        readonly static object _Locker = new object();

        readonly IEnumerable<IModuleClaimProvider> _ClaimsProviders;
        #endregion

        /// <summary>
        /// /
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        public JsonRoleRepository(string path, string fileName, IEnumerable<IModuleClaimProvider> claimsProviders)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            this._Path = path;
            this._FileName = fileName;
            this._ClaimsProviders = claimsProviders ?? new List<IModuleClaimProvider>();
        }

        public IEnumerable<RoleDefinitionDto> GetAll()
        {
            var jsonString = "";
            if (!File.Exists(Path.Combine(this._Path, this._FileName)))
                return new List<RoleDefinitionDto>();

            //persist data
            lock (_Locker)
            {
                jsonString = File.ReadAllText(Path.Combine(this._Path, this._FileName));
            }
            var options = new JsonSerializerOptions { WriteIndented = true };
            return JsonSerializer.Deserialize<List<RoleDefinitionDto>>(jsonString, options) ?? new List<RoleDefinitionDto>();
        }

        public RoleDefinitionDto? GetByKey(string role)
        {
            if (string.IsNullOrEmpty(role))
                throw new ArgumentNullException(nameof(role));

            return GetAll().SingleOrDefault(x => x.Role == role);
        }

        public void Save(RoleDefinitionDto role)
        {
            if (role is null)
                throw new ArgumentNullException(nameof(role));
            if (string.IsNullOrEmpty(role.Role))
                throw new ArgumentNullException(nameof(role));

            var roles = this.GetAll().ToList();

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
                Claims = new List<ClaimDefinitionDTO>()
                {
                    new ClaimDefinitionDTO(){Type="https://umbrella/adminTools", Value="W" },
                }
            });

            // add new claims for existing roles
            foreach(var provider in this._ClaimsProviders)
            {
                var claimsPerRoles = provider.GetClaims();
                foreach(var role in claimsPerRoles)
                {
                    var existingRole = list.SingleOrDefault(x => x.Role.ToLowerInvariant() == role.Key.ToUpperInvariant());
                    if(existingRole == null)
                        continue;

                    foreach(var claim in role.Value)
                    {
                        if(!existingRole.Claims.Any(x => x.Type == claim.Type))
                            existingRole.Claims.Add(claim);
                    }
                }
            }
            SaveListOfRoles(list);
        }

        #region Protected Methods

        protected void SaveListOfRoles(List<RoleDefinitionDto> roles)
        {
            //serialize list
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(roles, options);

            //persist data
            lock (_Locker)
            {
                File.WriteAllText(Path.Combine(this._Path, this._FileName), jsonString);
            }

        }

        #endregion


    }
}