using NSubstitute;
using Umbrella.IdentityManagement.Roles;

namespace Umbrella.IdentityManagement.Tests.Roles
{
    public class RoleRepositoryFactoryTests
    {

        [Test]
        public void Constr_ThrowsEx_IfRepositoriesIsNull()
        {
            //********** GIVEN

            //*********** WHEN
            void test() => new RoleRepositoryFactory(null);

            //*********** Assert
            Assert.Throws<ArgumentNullException>(test);
            Assert.Pass();
        }

        [Test]
        public void Constr_ThrowsEx_IfRepositoriesIsEmpty()
        {
            //********** GIVEN

            //*********** WHEN
            void test() => new RoleRepositoryFactory(new List<IRoleRepository>());

            //*********** Assert
            Assert.Throws<ArgumentNullException>(test);
            Assert.Pass();
        }

        [Test]
        public void Build_ThrowsEx_IfApplicationidIsNull()
        {
            //********** GIVEN
            var repo = Substitute.For<IRoleRepository>();
            var factory = new RoleRepositoryFactory(new List<IRoleRepository>() { repo });

            //*********** WHEN
            void test() => factory.Build(null);

            //*********** Assert
            Assert.Throws<ArgumentNullException>(test);
            Assert.Pass();
        }

        [Test]
        public void Build_ThrowsEx_IfNoRepositoryIsFoundForTargetApplication()
        {
            //********** GIVEN
            string applicationId = "not-existing-app";
            var repo = Substitute.For<IRoleRepository>();
            var factory = new RoleRepositoryFactory(new List<IRoleRepository>() { repo });

            //*********** WHEN
            void test() => factory.Build(applicationId);

            //*********** Assert
            Assert.Throws<InvalidOperationException>(test);
            Assert.Pass();
        }

        [Test]
        public void Build_ThrowsEx_IfMoreTHanOneRepoIsFoundForTargetApplication()
        {
            //********** GIVEN
            string applicationId = "app";
            var repo = Substitute.For<IRoleRepository>();
            repo.ApplicationId.Returns(applicationId);
            var factory = new RoleRepositoryFactory(new List<IRoleRepository>() { repo, repo });

            //*********** WHEN
            void test() => factory.Build(applicationId);

            //*********** Assert
            Assert.Throws<InvalidOperationException>(test);
            Assert.Pass();
        }

        [Test]
        public void Build_Returns_RepoForTargetApp()
        {
            //********** GIVEN
            string applicationId = "app";
            var repo = Substitute.For<IRoleRepository>();
            repo.ApplicationId.Returns(applicationId);
            var factory = new RoleRepositoryFactory(new List<IRoleRepository>() { repo });

            //*********** WHEN
            var newRepo = factory.Build(applicationId);

            //*********** Assert
            Assert.IsNotNull(newRepo);
            Assert.AreEqual(applicationId, newRepo.ApplicationId);
            Assert.Pass();
        }
    }
}

