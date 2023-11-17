
using Microsoft.Extensions.Logging;
using NSubstitute;
using Umbrella.IdentityManagement.Claims;
using Umbrella.IdentityManagement.ClientAuthentication;
using Umbrella.IdentityManagement.ErrorManagement;
using Umbrella.IdentityManagement.TokenManagement;
using Umbrella.IdentityManagement.TokenManagement.Configuration;
using Umbrella.UserManagement;

namespace Umbrella.IdentityManagement.Tests.TokenManagement
{
    internal class IdentityServiceTests
    {
        #region
        IIdentityService _Service;
        IUserRepository _UserRepository;
        IClaimProvider _ClaimProvider;
        JwtSettings _Options;
        IEnumerable<ClientSettings> _ClientSettings;
        #endregion

        [SetUp]
        public void Setup()
        {
            this._UserRepository = Substitute.For<IUserRepository>();

            this._ClaimProvider = Substitute.For<IClaimProvider>();

            this._Options = new JwtSettings()
            {
                ValidAudience = "my-audience",
                TokenValidityInMinutes = 3600,
                ValidIssuer = "my-issuer",
                SigningKey = "this is my custom Secret key for authentication"
            };

            this._ClientSettings = new List<ClientSettings>()
            {
                new ClientSettings()
                {
                     ApplicationID = "app",
                     ClientID ="client1",
                     SecretID = "test",
                      Name = "test"
                }
            };

            this._Service = new JWTIdentityService(Substitute.For<ILogger>(),
                                                this._UserRepository,
                                                this._ClaimProvider,
                                                this._Options,
                                                this._ClientSettings);
        }

        [Test]
        public void Constr_ThrowsEx_IfLoggerIsNull()
        {
            //********** GIVEN

            //*********** WHEN
            void test() => new JWTIdentityService(null,
                                                this._UserRepository,
                                                this._ClaimProvider,
                                                this._Options,
                                                this._ClientSettings);

            //*********** Assert
            Assert.Throws<ArgumentNullException>(test);
            Assert.Pass();
        }

        [Test]
        public void Constr_ThrowsEx_IfClaimProviderIsNull()
        {
            //********** GIVEN

            //*********** WHEN
            void test() => new JWTIdentityService(Substitute.For<ILogger>(),
                            this._UserRepository,
                                                null,
                                                this._Options,
                                                this._ClientSettings);

            //*********** Assert
            Assert.Throws<ArgumentNullException>(test);
            Assert.Pass();
        }

        [Test]
        public void Constr_ThrowsEx_IfOptionsIsNull()
        {
            //********** GIVEN

            //*********** WHEN
            void test() => new JWTIdentityService(Substitute.For<ILogger>(), this._UserRepository,
                                                this._ClaimProvider,
                                                null,
                                                this._ClientSettings);

            //*********** Assert
            Assert.Throws<ArgumentNullException>(test);
            Assert.Pass();
        }

        [Test]
        public void Constr_ThrowsEx_IfClientsIsNull()
        {
            //********** GIVEN

            //*********** WHEN
            void test() => new JWTIdentityService(Substitute.For<ILogger>(),
                                                 this._UserRepository, 
                                                 this._ClaimProvider,
                                                this._Options,
                                                null);

            //*********** Assert
            Assert.Throws<ArgumentNullException>(test);
            Assert.Pass();
        }

        [Test]
        public void Constr_ThrowsEx_IfClientsIsEmpty()
        {
            //********** GIVEN

            //*********** WHEN
            void test() => new JWTIdentityService(Substitute.For<ILogger>(),
                                                 this._UserRepository, 
                                                 this._ClaimProvider,
                                                this._Options,
                                                new List<ClientSettings>());

            //*********** Assert
            Assert.Throws<ArgumentNullException>(test);
            Assert.Pass();
        }

        [Test]
        public void AuthenticateClient_ThrowsEx_IfClientsIsEmpty()
        {
            //********** GIVEN

            //*********** WHEN
            void test() => this._Service.AuthenticateClient(null, null);

            //*********** Assert
            Assert.Throws<ArgumentNullException>(test);
            Assert.Pass();
        }

        [Test]
        public void AuthenticateClient_ThrowsEx_IfSecretIsEmpty()
        {
            //********** GIVEN

            //*********** WHEN
            void test() => this._Service.AuthenticateClient("client", null);

            //*********** Assert
            Assert.Throws<ArgumentNullException>(test);
            Assert.Pass();
        }

        [Test]
        public void AuthenticateClient_ThrowsEx_IfClientNotFound()
        {
            //********** GIVEN

            //*********** WHEN
            void test() => this._Service.AuthenticateClient("not-existing-client", "test");

            //*********** Assert
            Assert.Throws<IdentityValidationException>(test);
            Assert.Pass();
        }
    }
}
