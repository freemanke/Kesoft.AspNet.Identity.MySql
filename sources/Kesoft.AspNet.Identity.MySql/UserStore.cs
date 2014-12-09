
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kesoft.AspNet.Identity.MySql
{
    /// <summary>
    /// Class that implements the key ASP.NET Identity user store iterfaces.
    /// </summary>
    public class UserStore :
        IUserClaimStore<IdentityUser>,
        IUserLoginStore<IdentityUser>,
        IUserRoleStore<IdentityUser>,
        IUserPasswordStore<IdentityUser>
    {
        private readonly bool contextDisposed;

        /// <summary>
        /// Ormlite db context.
        /// </summary>
        public IdentityContext Context { get; private set; }

        /// <summary>
        /// Constructor that takes a IdentityContext as argument.
        /// </summary>
        /// <param name="context">Db context.</param>
        /// <param name="contextDisposed">If true, context will be disposed, default value is false.</param>
        public UserStore(IdentityContext context, bool contextDisposed = false)
        {
            Context = context;
            this.contextDisposed = contextDisposed;
        }

        /// <summary>
        /// Insert a new IdentityUser in the UserTable.
        /// </summary>
        /// <param name="user">User.</param>
        /// <returns></returns>
        public Task CreateAsync(IdentityUser user)
        {
            if (user == null)   throw new ArgumentNullException("user");
            Context.Users.Add(user);

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Returns an IdentityUser instance based on a userId query.
        /// </summary>
        /// <param name="userId">The user's Id.</param>
        /// <returns></returns>
        public Task<IdentityUser> FindByIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentException("Null or empty argument: userId");

            var result = Context.Users.FirstOrDefault(a => a.Id == userId);
            return result != null ? Task.FromResult(result) : Task.FromResult<IdentityUser>(null);
        }

        /// <summary>
        /// Returns an IdentityUser instance based on a userName query.
        /// </summary>
        /// <param name="userName">The user's name.</param>
        /// <returns></returns>
        public Task<IdentityUser> FindByNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))throw new ArgumentException("Null or empty argument: userName");
            var result = Context.Users.FirstOrDefault(a => a.UserName == userName);
            return result != null ? Task.FromResult(result) : Task.FromResult<IdentityUser>(null);
        }

        /// <summary>
        /// Updates the UsersTable with the IdentityUser instance values.
        /// </summary>
        /// <param name="user">IdentityUser to be updated.</param>
        /// <returns></returns>
        public Task UpdateAsync(IdentityUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            Context.Users.Update(user);
            return Task.FromResult<object>(null);
        }

        public virtual void Dispose()
        {
            if (Context != null && contextDisposed)
            {
                Context.Dispose();
                Context = null;
            }
        }

        /// <summary>
        /// Inserts a claim to the UserClaims table for the given user.
        /// </summary>
        /// <param name="user">User to have claim added.</param>
        /// <param name="claim">Claim to be added.</param>
        /// <returns></returns>
        public Task AddClaimAsync(IdentityUser user, Claim claim)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (claim == null) throw new ArgumentNullException("claim");
            Context.UserClaims.Add(new IdentityUserClaim {ClaimType = claim.Type, ClaimValue = claim.Value,  UserId=user.Id});

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Returns all claims for a given user.
        /// </summary>
        /// <param name="user">User.</param>
        /// <returns></returns>
        public Task<IList<Claim>> GetClaimsAsync(IdentityUser user)
        {
            var claims = Context.UserClaims.ToList(a => a.UserId == user.Id).Select(b => new Claim(b.ClaimType, b.ClaimValue));
            return Task.FromResult<IList<Claim>>(claims.ToList());
        }

        /// <summary>
        /// Removes a claim froma user.
        /// </summary>
        /// <param name="user">User to have claim removed.</param>
        /// <param name="claim">Claim to be removed.</param>
        /// <returns></returns>
        public Task RemoveClaimAsync(IdentityUser user, Claim claim)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (claim == null)  throw new ArgumentNullException("claim");
           
            Context.UserClaims.Remove(a => a.UserId == user.Id && a.ClaimValue == claim.Value && a.ClaimType == claim.Type);

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Inserts a Login in the UserLogins's table for a given User.
        /// </summary>
        /// <param name="user">User to have login added.</param>
        /// <param name="login">Login to be added.</param>
        /// <returns></returns>
        public Task AddLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (login == null) throw new ArgumentNullException("login");
            
            Context.UserLogins.Add(new IdentityUserLogin {UserId = user.Id, ProviderKey = login.ProviderKey, LoginProvider = login.LoginProvider});

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Returns an IdentityUser based on the Login info
        /// </summary>
        /// <param name="login">Login info.</param>
        /// <returns></returns>
        public Task<IdentityUser> FindAsync(UserLoginInfo login)
        {
            if (login == null)  throw new ArgumentNullException("login");

            var userLogin = Context.UserLogins.FirstOrDefault(a => a.LoginProvider == login.LoginProvider && a.ProviderKey == login.ProviderKey);
            if (userLogin != null)
            {
                var userId = userLogin.UserId;
                if (userId != null)
                {
                    var user = Context.Users.FirstOrDefault(a => a.Id == userId);
                    if (user != null) return Task.FromResult(user);
                }
            }

            return Task.FromResult<IdentityUser>(null);
        }

        /// <summary>
        /// Returns list of UserLoginInfo for a given IdentityUser.
        /// </summary>
        /// <param name="user">User.</param>
        /// <returns></returns>
        public Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user)
        {
            if (user == null) throw new ArgumentNullException("user");

            var logins = Context.UserLogins.ToList(a => a.UserId == user.Id).Select(b => new UserLoginInfo(b.ProviderKey, b.LoginProvider)).ToList();

            return Task.FromResult<IList<UserLoginInfo>>(logins);
        }

        /// <summary>
        /// Deletes a login from UserLoginsTable for a given IdentityUser
        /// </summary>
        /// <param name="user">User to have login removed.</param>
        /// <param name="login">Login to be removed.</param>
        /// <returns></returns>
        public Task RemoveLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            if (user == null)  throw new ArgumentNullException("user");
            if (login == null)  throw new ArgumentNullException("login");

            Context.UserLogins.Remove(a => a.UserId == user.Id && a.LoginProvider == login.LoginProvider && a.ProviderKey == login.ProviderKey);

            return Task.FromResult<Object>(null);
        }

        /// <summary>
        /// Inserts a entry in the UserRoles table
        /// </summary>
        /// <param name="user">User to have role added.</param>
        /// <param name="roleName">Name of the role to be added to user.</param>
        /// <returns></returns>
        public Task AddToRoleAsync(IdentityUser user, string roleName)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (string.IsNullOrEmpty(roleName)) throw new ArgumentException("Argument cannot be null or empty: roleName.");

            var role = Context.Roles.FirstOrDefault(a => a.Name == roleName);
            if (role != null) throw new Exception(string.Format("Role name {0} is already token.", roleName));
           
            role = new IdentityRole {Name = roleName};
            Context.UserInRoles.Add(new IdentityUserInRole {RoleId = role.Id, UserId = user.Id});

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Returns the roles for a given IdentityUser
        /// </summary>
        /// <param name="user">User.</param>
        /// <returns></returns>
        public Task<IList<string>> GetRolesAsync(IdentityUser user)
        {
            if (user == null) throw new ArgumentNullException("user");

            var roleIds = Context.UserInRoles.ToList(a => a.UserId == user.Id).Select(a => a.RoleId.ToString()).ToList();
            var roleNames = new List<string>();
            roleIds.ForEach(a => roleNames.Add(Context.Roles.FirstOrDefault(b => b.Id == a).Name));

            return Task.FromResult<IList<string>>(roleNames);
        }

        /// <summary>
        /// Verifies if a user is in a role
        /// </summary>
        /// <param name="user">User.</param>
        /// <param name="role">Role name.</param>
        /// <returns></returns>
        public Task<bool> IsInRoleAsync(IdentityUser user, string role)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (string.IsNullOrEmpty(role))  throw new ArgumentNullException("role");

            var r = Context.Roles.FirstOrDefault(a => a.Name == role);
            if (r == null) return Task.FromResult(false);
            var userInRole = Context.UserInRoles.FirstOrDefault(a => a.UserId == user.Id && a.RoleId == r.Id);
            
            return Task.FromResult(userInRole != null);
        }

        /// <summary>
        /// Removes a user from a role
        /// </summary>
        /// <param name="user">User.</param>
        /// <param name="role">Role name.</param>
        /// <returns></returns>
        public Task RemoveFromRoleAsync(IdentityUser user, string role)
        {
            var r = Context.Roles.FirstOrDefault(a => a.Name == role);
            if (r != null) Context.UserInRoles.Remove(a => a.UserId == user.Id && a.RoleId == r.Id);

            return null;
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="user">User.</param>
        /// <returns></returns>
        public Task DeleteAsync(IdentityUser user)
        {
            if (user != null) Context.Users.Remove(user.Id);

            return Task.FromResult<Object>(null);
        }

        /// <summary>
        /// Returns the PasswordHash for a given IdentityUser.
        /// </summary>
        /// <param name="user">User.</param>
        /// <returns></returns>
        public Task<string> GetPasswordHashAsync(IdentityUser user)
        {
            var id = user.Id;
            user = Context.Users.FirstOrDefault(a => a.Id == id);
            return Task.FromResult(user != null ? user.PasswordHash : null);
        }

        /// <summary>
        /// Verifies if user has password.
        /// </summary>
        /// <param name="user">User.</param>
        /// <returns></returns>
        public Task<bool> HasPasswordAsync(IdentityUser user)
        {
            var id = user.Id;
            user = Context.Users.FirstOrDefault(a => a.Id == id);
            var hasPassword = user != null && !string.IsNullOrEmpty(user.PasswordHash);

            return Task.FromResult(hasPassword);
        }

        /// <summary>
        /// Sets the password hash for a given IdentityUser.
        /// </summary>
        /// <param name="user">User.</param>
        /// <param name="passwordHash">User's password hash.</param>
        /// <returns></returns>
        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;

            return Task.FromResult<Object>(null);
        }
    }
}