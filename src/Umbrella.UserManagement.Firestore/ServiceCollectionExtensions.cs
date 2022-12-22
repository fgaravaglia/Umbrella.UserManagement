using Umbrella.UserManagement;
using Umbrella.Infrastructure.Firestore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Umbrella.UserManagement.Firestore
{
    internal static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the FIrestore implementation for User repository
        /// </summary>
        /// <param name="services"></param>
        /// <param name="gcpProjectId"></param>
        /// <param name="environmentName"></param>
        public static void AddFirestoreUserManagement(this IServiceCollection services, string gcpProjectId, string environmentName)
        {
            if (String.IsNullOrEmpty(gcpProjectId))
                throw new ArgumentNullException(nameof(gcpProjectId));
            if (String.IsNullOrEmpty(environmentName))
                throw new ArgumentNullException(nameof(environmentName));

            services.AddTransient<IUserRepository, FirestoreUserRepository>(x =>
            {
                var logger = x.GetRequiredService<ILogger>();
                return new FirestoreUserRepository(logger, gcpProjectId, environmentName);
            });
        }
    }
}