using Microsoft.Extensions.DependencyInjection;

namespace Umbrella.Security
{
    /// <summary>
    /// Extensions to set up services at startup
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Injects into DI the services to manage user
        /// </summary>
        /// <param name="services"></param>
        /// <param name="staticDataPath"></param>
        public static void AddUserManagement(this IServiceCollection services, string staticDataPath)
        {
            if(string.IsNullOrEmpty(staticDataPath))
                throw new ArgumentNullException(nameof(staticDataPath));

            services.AddSingleton<IUserRepository>(x => new JsonUserRepository(staticDataPath, "Users.json"));
        }
    }
}