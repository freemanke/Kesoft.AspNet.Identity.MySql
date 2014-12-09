using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Kesoft.AspNet.Identity.MySql;
using Kesoft.OrmLite.MySql;
using Microsoft.AspNet.Identity;
using ServiceStack.OrmLite;

namespace Kesoft.AspNet.Identity.MySql
{
    /// <summary>
    /// Identity Db context.
    /// </summary>
    public class IdentityContext : MySqlContext
    {
        private IRepository<IdentityUser> users;
        private IRepository<IdentityRole> roles;
        private IRepository<IdentityUserInRole> userInRoles;
        private IRepository<IdentityUserClaim> claims;
        private IRepository<IdentityUserLogin> logins;

        /// <summary>
        /// Default constructor uses DefaultConnection.
        /// </summary>
        public IdentityContext()
            : this(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString)
        {
        }

        /// <summary>
        /// Constructor uses connectionString as parameter.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        public IdentityContext(string connectionString) 
            : base(connectionString)
        {
        }

        /// <summary>
        /// Drops and creates all tables.
        /// </summary>
        public override void DropCreateTables()
        {
            Connection.DropTables(
                typeof (IdentityUserInRole), typeof (IdentityUserClaim), typeof (IdentityUserLogin),
                typeof (IdentityUser), typeof (IdentityRole));

            Connection.CreateTables(true,
                typeof (IdentityUser), typeof (IdentityRole), typeof (IdentityUserInRole),
                typeof (IdentityUserClaim), typeof (IdentityUserLogin));
        }

        /// <summary>
        /// User repository.
        /// </summary>
        public IRepository<IdentityUser> Users
        {
            get { return users ?? (users = new Repository<IdentityUser>(Connection)); }
        }

        /// <summary>
        /// Role repository.
        /// </summary>
        public IRepository<IdentityRole> Roles
        {
            get { return roles ?? (roles = new Repository<IdentityRole>(Connection)); }
        }

        /// <summary>
        /// UserInRoles repository.
        /// </summary>
        public IRepository<IdentityUserInRole> UserInRoles
        {
            get { return userInRoles ?? (userInRoles = new Repository<IdentityUserInRole>(Connection)); }
        }

        /// <summary>
        /// UserClaims repository.
        /// </summary>
        public IRepository<IdentityUserClaim> UserClaims
        {
            get { return claims ?? (claims = new Repository<IdentityUserClaim>(Connection)); }
        }

        /// <summary>
        /// UserLogins repository.
        /// </summary>
        public IRepository<IdentityUserLogin> UserLogins
        {
            get { return logins ?? (logins = new Repository<IdentityUserLogin>(Connection)); }
        }
    }
}