using Microsoft.Extensions.Logging;
using Umbrella.UserManagement;
using Umbrella.Infrastructure.Firestore;
using Umbrella.Infrastructure.Firestore.Abstractions;

namespace Umbrella.UserManagement.Firestore
{
    /// <summary>
    /// Firestore implemention of IUserRepository
    /// </summary>
    public class FirestoreUserRepository : ModelEntityRepository<UserDto, UserFirestoreDocument>, IUserRepository
    {
        /// <summary>
        /// Default Constr
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="projectId"></param>
        /// <param name="dotnetEnv"></param>
        public FirestoreUserRepository(ILogger logger, 
                                        IFirestoreDocMapper<UserDto, UserFirestoreDocument> mapper,
                                        IFirestoreDataRepository<UserFirestoreDocument> firestoreRepo)
            : base(logger, mapper, firestoreRepo)
        {
        }

        /// <summary>
        /// gets the specific user
        /// </summary>
        /// <param name="name">user name</param>
        /// <returns></returns>
       public  UserDto GetByKey(string name)
       {
            return this.GetById(name);
       }
        /// <summary>
        /// saves the user
        /// </summary>
        /// <param name="user"></param>
        new public void Save(UserDto user)
        {
            base.Save(user);
        }
        /// <summary>dotnet build
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

            var user = this.GetById(name);
            if (user is null)
            {
                user = new UserDto()
                {
                    Name = name,
                    DisplayName = displayName,
                    LastLoginDate = DateTime.Now
                };
            }
            user.DisplayName = displayName;
            user.LastLoginDate = DateTime.Now;
            user.SetImageUrl(imageUrl);

            this.Save(user);
        }
        /// <summary>
        /// Saves the list of roles associated to the current user
        /// </summary>
        /// <param name="name">user to update</param>
        /// <param name="roles">list of roles to associate to user</param>
        public void SaveRoles(string name, List<string> roles)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            if (roles == null)
                throw new ArgumentNullException(nameof(roles));
            if (!roles.Any())
                throw new ArgumentNullException(nameof(roles));

            var user = this.GetById(name);
            if (user is null)
                throw new NullReferenceException($"User {name} not found");

            // set roles
            user.Roles.Clear();
            user.Roles.AddRange(roles.Distinct());
            this.Save(user);
        }
    }
}