
namespace Umbrella.IdentityManagement.Claims
{
    /// <summary>
    /// Abstraction of component to inject claims from a module
    /// </summary>
    public interface IModuleClaimProvider
    {
        /// <summary>
        /// MOdule Name that provides roles
        /// </summary>
        /// <value></value>
        string ModuleName { get; }

        /// <summary>
        /// Gest the claims for each role
        /// </summary>
        /// <returns></returns>
        Dictionary<string, List<ClaimDefinitionDto>> GetClaimsPerRole();
    }
}