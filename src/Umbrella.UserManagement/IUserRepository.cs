using System.Collections.Generic;

namespace Umbrella.Security
{
    /// <summary>
    /// Abstraction for Storage of User definitions
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// GEts all avaialble users
        /// </summary>
        /// <returns></returns>
        IEnumerable<UserDTO> GetAll();
        /// <summary>
        /// gets the specific user
        /// </summary>
        /// <param name="name">user name</param>
        /// <returns></returns>
        UserDTO GetByKey(string name);
        /// <summary>
        /// saves the user
        /// </summary>
        /// <param name="user"></param>
        void Save(UserDTO user);
        /// <summary>
        /// Refreshes the details aftaer each login
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <param name="imageUrl"></param>
        void UpdateDetailsAfterLogin(string name, string displayName, string imageUrl);
        /// <summary>
        /// Saves the list of roles associated to the current user
        /// </summary>
        /// <param name="name">user to update</param>
        /// <param name="roles">list of roles to associate to user</param>
        void SaveRoles(string name, List<string> roles);
    }
}