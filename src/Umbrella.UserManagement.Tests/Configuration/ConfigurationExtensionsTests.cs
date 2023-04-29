using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Umbrella.UserManagement.Configuration;

namespace Umbrella.UserManagement.Tests.Configuration
{
    public class ConfigurationExtensionsTests
    {
        [Test]
        public void GetUserManagementSettings_ThrowsEx_IfConfigIsNull()
        {
            //****** GIVEN
            IConfiguration config = null;

            //****** WHEN
            void testCode() => config.GetUserManagementSettings();

            //****** ASSERT
            Assert.Throws<ArgumentNullException>(testCode);
            Assert.Pass();
        }
    }
}