using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Umbrella.UserManagement
{
    /// <summary>
    /// SImple implementation of Repo for users based on json file
    /// </summary>
    public class JsonUserRepository : IUserRepository
    {
        #region Attributes
        readonly string _Path;
        readonly string _FileName;
        static object _Locker = new object();
        #endregion

        /// <summary>
        /// Default constr
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        public JsonUserRepository(string path, string fileName)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            this._Path = path;
            this._FileName = fileName;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserDto> GetAll()
        {
            var list = new List<UserDto>();
            string jsonString = "";
            if (!File.Exists(Path.Combine(this._Path, this._FileName)))
            {

                //serialize list
                string js = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });

                //persist data
                lock (_Locker)
                {
                    File.WriteAllText(Path.Combine(this._Path, this._FileName), js);
                }

                return list;
            }


            //persist data
            lock (_Locker)
            {
                jsonString = File.ReadAllText(Path.Combine(this._Path, this._FileName));
            }
            var options = new JsonSerializerOptions { WriteIndented = true };
            list = JsonSerializer.Deserialize<List<UserDto>>(jsonString, options);

            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public UserDto GetByKey(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            return GetAll().SingleOrDefault(x => x.Name == name);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public void Save(UserDto user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.Name))
                throw new ArgumentNullException(nameof(user), "Name cannot be null");

            var roles = this.GetAll().ToList();

            var existingItem = roles.SingleOrDefault(x => x.Name == user.Name);
            if (existingItem == null)
                roles.Add(existingItem);
            else
            {
                existingItem.DisplayName = user.DisplayName;
                existingItem.SetImageUrl(user.ImageUrl);
                existingItem.LastLoginDate = user.LastLoginDate;
                existingItem.Roles.Clear();
                existingItem.Roles.AddRange(user.Roles);
            }

            //serialize list
            SaveAll(roles);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="users"></param>
        public void SaveAll(IEnumerable<UserDto> users)
        {
            //serialize list
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(users, options);

            //persist data
            lock (_Locker)
            {
                File.WriteAllText(Path.Combine(this._Path, this._FileName), jsonString);
            }
        }
        /// <summary>
        /// Refreshes the details aftaer each login
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <param name="imageUrl"></param>
        public void UpdateDetailsAfterLogin(string name, string displayName, string imageUrl)
        {
            if (string.IsNullOrEmpty(displayName))
                throw new ArgumentNullException(nameof(displayName));
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(imageUrl))
                throw new ArgumentNullException(nameof(imageUrl));

            var users = this.GetAll().ToList();
            var user = users.SingleOrDefault(x => x.Name == name);
            if (user is null)
            {
                user = new UserDto()
                {
                    Name = name,
                    DisplayName = displayName,
                    LastLoginDate = DateTime.Now
                };
                users.Add(user);
            }
            user.DisplayName = displayName;
            user.LastLoginDate = DateTime.Now;
            user.SetImageUrl(imageUrl);

            //serialize list
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(users, options);

            //persist data
            lock (_Locker)
            {
                File.WriteAllText(Path.Combine(this._Path, this._FileName), jsonString);
            }
        }

        /// <summary>
        /// Saves the list of roles associated to the current user
        /// </summary>
        /// <param name="name"></param>
        /// <param name="roles"></param>
        public void SaveRoles(string name, List<string> roles)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            if (roles == null)
                throw new ArgumentNullException(nameof(roles));
            if (!roles.Any())
                throw new ArgumentNullException(nameof(roles));

            var users = this.GetAll().ToList();
            var user = users.SingleOrDefault(x => x.Name == name);
            if (user is null)
                throw new ApplicationException($"User {name} not found");

            // set roles
            user.Roles.Clear();
            user.Roles.AddRange(roles.Distinct());

            //serialize list
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(users, options);

            //persist data
            lock (_Locker)
            {
                File.WriteAllText(Path.Combine(this._Path, this._FileName), jsonString);
            }
        }
    }
}