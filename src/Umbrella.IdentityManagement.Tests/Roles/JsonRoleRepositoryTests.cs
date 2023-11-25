using NSubstitute;
using Umbrella.IdentityManagement.Claims;
using Umbrella.IdentityManagement.Roles;
using Umbrella.IdentityManagement.Roles.Providers;

namespace Umbrella.IdentityManagement.Tests.Roles
{
    public class JsonRoleRepositoryTests
    {
        string _TestFolderPath;
        IRoleRepository _Repo;

        [SetUp]
        public void Setup()
        {
            this._TestFolderPath = Path.Combine(Environment.CurrentDirectory, nameof(JsonRoleRepositoryTests));
            if (!Directory.Exists(this._TestFolderPath))
                Directory.CreateDirectory(this._TestFolderPath);

            this._Repo = new JsonRoleRepository("APP", this._TestFolderPath);
        }

        [TearDown]
        public void TearDown()
        {
            var files = new DirectoryInfo(this._TestFolderPath).GetFiles();
            foreach (var f in files)
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
        public void Save_Generates_ExpectedJsonFile()
        {
            //********** GIVEN
            var role = new RoleDefinitionDto()
            {
                Role = "MYROLE",
                Claims = new List<ClaimDefinitionDto>()
                {
                    new ClaimDefinitionDto(){ Type="https://umbrella/invoke-api", Value="R"},
                    new ClaimDefinitionDto(){ Type="https://umbrella/invoke-api/prod", Value="R"}
                } 
            };

            //*********** WHEN
            this._Repo.Save(role);

            //*********** Assert
            Assert.IsTrue(File.Exists(Path.Combine(this._TestFolderPath, "APP-Roles.json")));
            Assert.Pass();
        }
    }
}