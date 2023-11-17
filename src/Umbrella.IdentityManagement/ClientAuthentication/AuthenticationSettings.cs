using System.Diagnostics.CodeAnalysis;

namespace Umbrella.IdentityManagement.ClientAuthentication
{
    /// <summary>
    /// Section Authetication from appSettings file
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AuthenticationSettings
    {
        /// <summary>
        /// List of IDs for enabled applications
        /// </summary>
        /// <value></value>
        public List<ClientSettings> Clients { get; set; }
        /// <summary>
        /// Valid Channels
        /// </summary>
        /// <value></value>
        public string ValidChannels { get; set; }
        /// <summary>
        /// EMpty COnstr
        /// </summary>
        public AuthenticationSettings()
        {
            this.Clients = new List<ClientSettings>();
            this.ValidChannels = "";
        }
    }

}