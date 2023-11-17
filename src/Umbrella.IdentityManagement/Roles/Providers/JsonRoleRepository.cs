using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Umbrella.IdentityManagement.Roles.Providers
{
    /// <summary>
    /// Concrete implementatoin of provider base don Json files
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class JsonRoleRepository : IRoleRepository
    {
        #region Fields

        readonly string _FolderPath;
        readonly string _FileName;
        readonly object _Locker = new object();
        readonly JsonSerializerOptions _SerializerOptions;
        readonly bool _AutogenerateSampleData;
        #endregion

        /// <summary>
        /// Id of application for which the repo is providing the roles
        /// </summary>
        public string ApplicationId { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="folderPath"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public JsonRoleRepository(string applicationId, string folderPath)
        {
            if (string.IsNullOrEmpty(applicationId))
                throw new ArgumentNullException(nameof(applicationId));
            this.ApplicationId = applicationId;

            if (string.IsNullOrEmpty(folderPath))
                throw new ArgumentNullException(nameof(folderPath));
            _FolderPath = folderPath;
            _FileName = $"{this.ApplicationId}-Roles.json";
            this._SerializerOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            this._AutogenerateSampleData = false;
        }

        /// <summary>
        /// <inheritdoc cref="IRoleRepository.GetAll"/>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleDefinitionDto> GetAll()
        {
            var fullPath = Path.Combine(this._FolderPath, _FileName);
            if (!File.Exists(fullPath))
            {
                if (this._AutogenerateSampleData)
                    InitializeRoles();
                return new List<RoleDefinitionDto>();
            }

            string jsonContent;
            lock (this._Locker)
            {
                jsonContent = File.ReadAllText(fullPath);
            }

            // deserialize 
            var roles = JsonSerializer.Deserialize<IEnumerable<RoleDefinitionDto>>(jsonContent, this._SerializerOptions);
            return roles ?? new List<RoleDefinitionDto>();
        }
        /// <summary>
        /// <inheritdoc cref="IRoleRepository.GetByKey(string)"/>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public RoleDefinitionDto? GetByKey(string key)
        {
            return this.GetAll().SingleOrDefault(x => x.Role == key);
        }
        /// <summary>
        /// <inheritdoc cref="IRoleRepository.Save(RoleDefinitionDto)"/>
        /// </summary>
        /// <param name="role"></param>
        /// <exception cref="NotImplementedException"></exception>
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
        /// <inheritdoc cref="IRoleRepository.InitializeRoles"/>
        /// </summary>
        public void InitializeRoles()
        {
            var roles = new List<RoleDefinitionDto>()
            {
                new RoleDefinitionDto()
                {
                    Role  ="CLIENT",
                    DisplayText = "Client for Api",
                    Claims = new List<Claims.ClaimDefinitionDto>()
                    {
                        new Claims.ClaimDefinitionDto()
                        {
                            Type = "https://umbrella/api", Value="W"
                        }
                    }
                },
                new RoleDefinitionDto()
                {
                    Role = "FANTATOURNAMENT-P",
                    CreatedOn= DateTime.Now,
                    DisplayText = "Client for Api",
                    Claims = new List<Claims.ClaimDefinitionDto>()
                    {
                        new Claims.ClaimDefinitionDto()
                        {
                            Type = "https://umbrella/fantatournament/prod", Value="R"
                        }
                    }
                },
                new RoleDefinitionDto()
                {
                    Role = "FANTATOURNAMENT-D",
                    CreatedOn= DateTime.Now,
                    DisplayText = "Client for Api",
                    Claims = new List<Claims.ClaimDefinitionDto>()
                    {
                        new Claims.ClaimDefinitionDto()
                        {
                            Type = "https://umbrella/fantatournament/dev", Value="R"
                        }
                    }
                }
            };

            var jsonContent = JsonSerializer.Serialize<IEnumerable<RoleDefinitionDto>>(roles, this._SerializerOptions);
            File.WriteAllText(Path.Combine(this._FolderPath, _FileName), jsonContent);
        }
    }
}
