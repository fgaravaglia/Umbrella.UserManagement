using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using Umbrella.IdentityManagement.Claims;
using Umbrella.IdentityManagement.ClientAuthentication;
using Umbrella.IdentityManagement.Roles;
using Umbrella.IdentityManagement.Roles.Providers;
using Umbrella.IdentityManagement.TokenManagement;
using Umbrella.IdentityManagement.TokenManagement.Configuration;
using Umbrella.UserManagement;

namespace Umbrella.IdentityManagement
{
    /// <summary>
    /// Estensions to register components into DI
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static  class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds dependencies for Identity
        /// </summary>
        /// <param name="services"></param>
        public static void AddIdentityServices(this IServiceCollection services)
        {
            services.AddTransient<IRoleRepositoryFactory, RoleRepositoryFactory>();
            services.AddTransient<IClaimProvider, ClaimProvider>();
            services.AddTransient<IIdentityService, JwtIdentityService>(x =>
            {
                var config = x.GetRequiredService<IConfiguration>();
                var logger = x.GetRequiredService<ILogger>();
                var claimProvider = x.GetRequiredService<IClaimProvider>();
                var userRepo = x.GetRequiredService<IUserRepository>();
                return new JwtIdentityService(logger, userRepo, claimProvider, config.GetJwtSettings(), config.GetClientSettings());
            });
        }
        /// <summary>
        /// Adds dependencies for Identity
        /// </summary>
        /// <param name="services"></param>
        public static void AddIdentityInfrastructureServices(this IServiceCollection services, IConfiguration config, string staticDataFolderPath)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (config == null) throw new ArgumentNullException(nameof(config));

            var applicationIds = config.GetClientSettings().Select(x => x.ApplicationID).Distinct().ToArray();
            services.AddIdentityInfrastructureServices(applicationIds, staticDataFolderPath);
        }

        /// <summary>
        /// Adds dependencies for Identity
        /// </summary>
        /// <param name="services"></param>
        public static void AddIdentityInfrastructureServices(this IServiceCollection services, string[] applicationIds, string staticDataFolderPath)
        {
            if (applicationIds == null)
                throw new ArgumentNullException(nameof(applicationIds));
            if (!applicationIds.Any())
                throw new ArgumentNullException(nameof(applicationIds));
            if (String.IsNullOrEmpty(staticDataFolderPath))
                throw new ArgumentNullException(nameof(staticDataFolderPath));

            // add here your dependencies
            foreach (var id in applicationIds)
                services.AddTransient<IRoleRepository, JsonRoleRepository>(x =>
                {
                    return new JsonRoleRepository(id, staticDataFolderPath);
                });
        }

    }
}
