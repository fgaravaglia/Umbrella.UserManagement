using NSubstitute;
using Umbrella.IdentityManagement.Roles;
using Umbrella.IdentityManagement.Roles.Providers;

namespace Umbrella.IdentityManagement.Tests.Roles
{
    public class RoleRepositoryFactoryTests
    {
        string _TestFolderPath;

        [SetUp]
        public void Setup()
        {
            this._TestFolderPath = Path.Combine(Environment.CurrentDirectory, nameof(RoleRepositoryFactoryTests));
            if(!Directory.Exists(this._TestFolderPath))
                Directory.CreateDirectory(this._TestFolderPath);
        }

        [TearDown]
        public void TearDown()
        {
            var files = new DirectoryInfo(this._TestFolderPath).GetFiles();
            foreach(var f in files)
            {
                try
                {
                    f.Delete();
                }
                catch
                {
                    Console.WriteLine("Error during deleting file " + f.Name);
                }
            }
        }

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

        [Test]
        public void Build_Returns_JsonRepoForTargetApp()
        {
            //********** GIVEN
            string applicationId = "app";
            var repo = new JsonRoleRepository(applicationId, this._TestFolderPath);
            var factory = new RoleRepositoryFactory(new List<IRoleRepository>() { repo });

            //*********** WHEN
            var newRepo = factory.Build(applicationId);

            //*********** Assert
            Assert.IsNotNull(newRepo);
            Assert.AreEqual(applicationId, newRepo.ApplicationId);
            Assert.That(newRepo, Is.TypeOf< JsonRoleRepository>());
            Assert.Pass();
        }

    }

}


