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
    public class IdentityContextTest
    {
        [Test]
        public void DropCreateLocalHost()
        {
            const string db = "DataSource=localhost;DataBase=Kesoft.AspNet.Identity.MySql;uid=root;pwd=fhit";
            var context = new IdentityContext(db);
            context.DropCreateTables();
        }
    }
}