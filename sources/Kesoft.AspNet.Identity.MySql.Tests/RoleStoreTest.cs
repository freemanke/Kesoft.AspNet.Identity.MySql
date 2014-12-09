using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using NUnit.Framework;
using ServiceStack.Common.Extensions;

namespace Kesoft.AspNet.Identity.MySql.Tests
{
    [TestFixture]
    public class RoleStoreTest
    {
        [Test]
        public void Add()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore(new IdentityContext()));
            var roleName = Guid.NewGuid().ToString();
            var role = new IdentityRole {Name = roleName};
            var result = roleManager.Create(role);
            Assert.IsTrue(result.Succeeded);

            role = roleManager.FindById(role.Id);
            Assert.IsNotNull(role);

            role = roleManager.FindByName(roleName);
            Assert.IsNotNull(role);

            Assert.IsTrue(roleManager.RoleExists(roleName));
            Assert.IsFalse(roleManager.RoleExists(Guid.NewGuid().ToString()));

            var newName = Guid.NewGuid().ToString();
            role.Name = newName;
            roleManager.Update(role);

            role = roleManager.FindById(role.Id);
            Assert.AreEqual(role.Name, newName);
        }
    }
}