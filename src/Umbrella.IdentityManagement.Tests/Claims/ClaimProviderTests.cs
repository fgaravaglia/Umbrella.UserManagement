using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Security.Claims;
using Umbrella.IdentityManagement.Claims;
using Umbrella.IdentityManagement.ErrorManagement;
using Umbrella.IdentityManagement.Roles;
using Umbrella.UserManagement;

namespace Umbrella.IdentityManagement.Tests.Claims
{
    public class ClaimProviderTests
    {
        #region Fields
        IClaimProvider _Provider;
        ILogger _Logger;
        IUserRepository _UserRepository;
        IRoleRepositoryFactory _RoleRepositoryFactory;
        IRoleRepository _RoleRepository;
        List<RoleDefinitionDto> _Roles;
        #endregion

        const string DEFAULT_USER_NAME = "user";
        const string ROLE_CLIENT = "CLIENT";

        [SetUp]
        public void Setup()
        {
            this._Logger = Substitute.For<ILogger>();
            this._UserRepository = Substitute.For<IUserRepository>();
            this._UserRepository.GetByKey(Arg.Is<string>(x => x == DEFAULT_USER_NAME)).Returns(new UserDto()
            {
                Name = DEFAULT_USER_NAME,
                DisplayName = "This is my user",
                CreationDate = DateTime.UtcNow,
                ImageUrl = "https://www.google.it",
                Roles = new List<string>() { "CLIENT" }
            });
            this._Roles = new List<RoleDefinitionDto>()
            {
                new RoleDefinitionDto()
                {
                    Role = ROLE_CLIENT,
                    Claims= new List<ClaimDefinitionDto>()
                    {
                        new ClaimDefinitionDto(){  Type = "https://Umbrella/login", Value="R"}
                    }
                },
                new RoleDefinitionDto()
                {
                    Role = ROLE_CLIENT + "2",
                    Claims= new List<ClaimDefinitionDto>()
                    {
                        new ClaimDefinitionDto(){  Type = "https://Umbrella/login", Value="R"},
                        new ClaimDefinitionDto(){  Type = "https://Umbrella/Repor", Value="R"}
                    }
                }
            };
            this._RoleRepository = Substitute.For<IRoleRepository>();
            this._RoleRepository.GetByKey(Arg.Is<string>(x => x == ROLE_CLIENT)).Returns(this._Roles[0]);
            this._RoleRepository.GetByKey(Arg.Is<string>(x => x == ROLE_CLIENT + "2")).Returns(this._Roles[1]);
            this._RoleRepositoryFactory = Substitute.For<IRoleRepositoryFactory>();
            this._RoleRepositoryFactory.Build(Arg.Any<string>()).Returns(this._RoleRepository);

            this._Provider = new ClaimProvider(this._Logger, this._UserRepository, this._RoleRepositoryFactory);
        }

        [Test]
        public void Constr_ThrowsEx_IfLoggerIsNull()
        {
            //********** GIVEN

            //*********** WHEN
            void test() => new ClaimProvider(null, this._UserRepository, this._RoleRepositoryFactory);

            //*********** Assert
            Assert.Throws<ArgumentNullException>(test);
            Assert.Pass();
        }

        [Test]
        public void Constr_ThrowsEx_IfLUserRepoIsNull()
        {
            //********** GIVEN

            //*********** WHEN
            void test() => new ClaimProvider(this._Logger, null, this._RoleRepositoryFactory);

            //*********** Assert
            Assert.Throws<ArgumentNullException>(test);
            Assert.Pass();
        }

        [Test]
        public void Constr_ThrowsEx_IfRoleRepoFactoryIsNull()
        {
            //********** GIVEN

            //*********** WHEN
            void test() => new ClaimProvider(this._Logger, this._UserRepository, null);

            //*********** Assert
            Assert.Throws<ArgumentNullException>(test);
            Assert.Pass();
        }

        [Test]
        public void GetByIdentityName_ThrowsEx_IfNameIsNull()
        {
            //********** GIVEN
            string name = "";
            string applicationId = "app";

            //*********** WHEN
            void test() => this._Provider.GetByIdentityName(name, applicationId);

            //*********** Assert
            Assert.Throws<ArgumentNullException>(test);
            Assert.Pass();
        }

        [Test]
        public void GetByIdentityName_ThrowsEx_IfAppIdIsNull()
        {
            //********** GIVEN
            string name = "name";
            string applicationId = "";

            //*********** WHEN
            void test() => this._Provider.GetByIdentityName(name, applicationId);

            //*********** Assert
            Assert.Throws<ArgumentNullException>(test);
            Assert.Pass();
        }

        [Test]
        public void GetByIdentityName_ThrowsEx_IfUserNotFoundl()
        {
            //********** GIVEN
            string name = "not-existing-user";
            string applicationId = "app";

            //*********** WHEN
            void test() => this._Provider.GetByIdentityName(name, applicationId);

            //*********** Assert
            Assert.Throws<IdentityValidationException>(test);
            Assert.Pass();
        }

        [Test]
        public void GetByIdentityName_Returns_ClaimForEachPermission()
        {
            //********** GIVEN
            string name = DEFAULT_USER_NAME;
            string applicationId = "app";
            var expectedRole = this._RoleRepository.GetByKey(ROLE_CLIENT);

            //*********** WHEN
            var claims = this._Provider.GetByIdentityName(name, applicationId);

            //*********** Assert
            Assert.IsTrue(claims.Any());
            AssertClaimExists(claims, ClaimTypes.NameIdentifier, name);
            AssertClaimExists(claims, ClaimTypes.Role, expectedRole.Role);
            foreach (var permission in expectedRole.Claims)
            {
                AssertClaimExists(claims, permission.Type, permission.Value);
            }
            Assert.Pass();
        }

        static void AssertClaimExists(IEnumerable<Claim> claims, string claimType, string expectedClaimValue)
        {
            var claim = claims.SingleOrDefault(x => x.Type == claimType);
            Assert.IsNotNull(claim, $"Unable to find claim of type {claimType}");
            Assert.That(claim.Value, Is.EqualTo(expectedClaimValue),
                            $"found wrong claim value for type {claimType}: found <{claim.Value}> instead of expected value <{expectedClaimValue}");
        }

        [Test]
        public void GetByIdentityName_Returns_ClaimForEachPermissionWithoutDuplicates()
        {
            //********** GIVEN
            string name = DEFAULT_USER_NAME;
            string applicationId = "app";
            this._UserRepository.GetByKey(Arg.Any<string>()).Returns(new UserDto()
            {
                Name = DEFAULT_USER_NAME,
                DisplayName = "This is my user",
                CreationDate = DateTime.UtcNow,
                ImageUrl = "https://www.google.it",
                Roles = new List<string>() { ROLE_CLIENT, ROLE_CLIENT + "2" }
            });

            //*********** WHEN
            var claims = this._Provider.GetByIdentityName(name, applicationId);

            //*********** Assert
            Assert.IsTrue(claims.Any());
            AssertClaimExists(claims, ClaimTypes.NameIdentifier, name);
            AssertClaimExists(claims, ClaimTypes.Role, ROLE_CLIENT + ";" + ROLE_CLIENT + "2");
            foreach (var role in this._Roles)
            {
                foreach (var permission in role.Claims)
                {
                    AssertClaimExists(claims, permission.Type, permission.Value);
                }
            }
            Assert.Pass();
        }
    }
}
