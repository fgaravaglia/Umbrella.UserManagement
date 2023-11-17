using Microsoft.Extensions.Configuration;
using Umbrella.IdentityManagement.ClientAuthentication;
using Umbrella.IdentityManagement.TokenManagement.Configuration;

namespace Umbrella.IdentityManagement.Tests.Configuration
{
    public class ConfigurationExtensionsTests
    {
        [Test]
        public void GetAuthenticationSettings__Returns_ExpectedSettings()
        {
            //********** GIVEN
            var appSettings = new Dictionary<string, string>();
            appSettings.Add("Authentication:ValidChannels", "client1;client2");
            appSettings.Add("Authentication:Clients:0:ClientID", "client1");
            appSettings.Add("Authentication:Clients:0:ApplicationID", "app1");
            appSettings.Add("Authentication:Clients:0:SecretID", "secret");
            appSettings.Add("Authentication:Clients:0:Name", "Client #1");
            appSettings.Add("Authentication:Clients:1:ClientID", "client2");
            appSettings.Add("Authentication:Clients:1:ApplicationID", "app1");
            appSettings.Add("Authentication:Clients:1:SecretID", "secret");
            appSettings.Add("Authentication:Clients:1:Name", "Client #1");
            IConfiguration config = ConfigurationManager.FromMemory(appSettings);

            //*********** WHEN
            var settings = config.GetAuthenticationSettings();

            //*********** Assert
            Assert.That(settings, Is.Not.Null);
            Assert.That(settings.ValidChannels, Is.EqualTo("client1;client2"));
            Assert.That(settings.Clients.ToList(), Has.Count.EqualTo(2));
            Assert.Pass();
        }

        [Test]
        public void GetAuthenticationSettings_ThrowsEx_IfNoSectionIsRead()
        {
            //********** GIVEN
            var appSettings = new Dictionary<string, string>();
            IConfiguration config = ConfigurationManager.FromMemory(appSettings);

            //*********** WHEN
            void test() => config.GetAuthenticationSettings();

            //*********** Assert
            Assert.Throws<InvalidOperationException>(test);
            Assert.Pass();
        }

        [Test]
        public void GetJwtOptions_ThrowsEx_IfNoOptionSectionIsRead()
        {
            //********** GIVEN
            var appSettings = new Dictionary<string, string>();
            IConfiguration config = ConfigurationManager.FromMemory(appSettings);

            //*********** WHEN
            void test() => config.GetJwtSettings();

            //*********** Assert
            Assert.Throws<InvalidOperationException>(test);
            Assert.Pass();
        }

        [Test]
        public void GetJwtSettings_Returns_ExpectedSettings()
        {
            //********** GIVEN
            var appSettings = new Dictionary<string, string>();
            appSettings.Add("Authentication:Jwt:ValidIssuer", "client1");
            appSettings.Add("Authentication:Jwt:ValidAudience", "app1");
            appSettings.Add("Authentication:Jwt:ValidateIssuer", "true");
            appSettings.Add("Authentication:Jwt:ValidateAudience", "true");
            appSettings.Add("Authentication:Jwt:SigningKey", "key");
            appSettings.Add("Authentication:Jwt:TokenValidityInMinutes", "15");
            IConfiguration config = ConfigurationManager.FromMemory(appSettings);

            //*********** WHEN
            var settings = config.GetJwtSettings();

            //*********** Assert
            Assert.That(settings, Is.Not.Null);
            Assert.That(settings.ValidIssuer, Is.EqualTo("client1"));
            Assert.That(settings.ValidAudience, Is.EqualTo("app1"));
            Assert.That(settings.SigningKey, Is.EqualTo("key"));
            Assert.That(settings.TokenValidityInMinutes, Is.EqualTo(15));
            Assert.That(settings.ValidateIssuer, Is.True);
            Assert.That(settings.ValidateAudience, Is.True);
            Assert.Pass();
        }

        [Test]
        public void GetClientSettings_ThrowsEx_IfNoClientSectionIsRead()
        {
            //********** GIVEN
            var appSettings = new Dictionary<string, string>();
            IConfiguration config = ConfigurationManager.FromMemory(appSettings);

            //*********** WHEN
            void test() => config.GetClientSettings();

            //*********** Assert
            Assert.Throws<InvalidOperationException>(test);
            Assert.Pass();
        }

        [Test]
        public void GetClientSettings_ThrowsEx_IfEmptyClientSectionIsRead()
        {
            //********** GIVEN
            var appSettings = new Dictionary<string, string>();
            appSettings.Add("Authentication:Clients", "");
            IConfiguration config = ConfigurationManager.FromMemory(appSettings);

            //*********** WHEN
            void test() => config.GetClientSettings();

            //*********** Assert
            Assert.Throws<InvalidOperationException>(test);
            Assert.Pass();
        }

        [Test]
        public void GetClientSettings_Returns_expectedClients()
        {
            //********** GIVEN
            var appSettings = new Dictionary<string, string>();
            appSettings.Add("Authentication:Clients:0:ClientID", "client1");
            appSettings.Add("Authentication:Clients:0:ApplicationID", "app1");
            appSettings.Add("Authentication:Clients:0:SecretID", "secret");
            appSettings.Add("Authentication:Clients:0:Name", "Client #1");
            appSettings.Add("Authentication:Clients:1:ClientID", "client2");
            appSettings.Add("Authentication:Clients:1:ApplicationID", "app1");
            appSettings.Add("Authentication:Clients:1:SecretID", "secret");
            appSettings.Add("Authentication:Clients:1:Name", "Client #1");
            IConfiguration config = ConfigurationManager.FromMemory(appSettings);

            //*********** WHEN
            var settings = config.GetClientSettings().ToList();

            //*********** Assert
            Assert.That(settings, Has.Count.EqualTo(2));
            Assert.Pass();
        }
    }
}
