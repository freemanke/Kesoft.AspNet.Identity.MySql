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
    public class UserStoreTest
    {
        [Test]
        public void All()
        {
            var userMgr = new UserManager<IdentityUser>(new UserStore(new IdentityContext()));

            var userName = Guid.NewGuid().ToString().Replace("-","");
            var password = userName;
            var user = userMgr.FindByName(userName);
            Assert.IsNull(user);
            user = new IdentityUser(userName);

            var result = userMgr.Create(user, password);
            Assert.IsTrue(result.Succeeded);

            user = userMgr.Find(userName, password);
            Assert.IsNotNull(user);
            Assert.AreEqual(user.UserName, userName);  
        }
    }
}