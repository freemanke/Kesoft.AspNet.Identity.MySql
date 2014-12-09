using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kesoft.AspNet.Identity.MySql
{
    /// <summary>
    /// Class that implements the key ASP.NET Identity role store iterfaces.
    /// </summary>
    public class RoleStore : IRoleStore<IdentityRole>
    {
        private readonly bool contextDisposed;
        public IdentityContext Context { get; private set; }

        /// <summary>
        /// Constructor that takes a IdentityContext as argument.
        /// </summary>
        /// <param name="context">Db context.</param>
        /// <param name="contextDisposed">If true, context will be disposed, default value is false.</param>
        public RoleStore(IdentityContext context, bool contextDisposed = false)
        {
            Context = context;
            this.contextDisposed = contextDisposed;
        }

        /// <summary>
        /// Creates a role.
        /// </summary>
        /// <param name="role">Role to be created.</param>
        /// <returns></returns>
        public Task CreateAsync(IdentityRole role)
        {
            if (role == null) throw new ArgumentNullException("role");

            var find = FindByNameAsync(role.Name).Result;
            if (find != null) throw new Exception("Role name is already token.");

            Context.Roles.Add(role);

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Deletes a role.
        /// </summary>
        /// <param name="role">Role to be deleted.</param>
        /// <returns></returns>
        public Task DeleteAsync(IdentityRole role)
        {
            if (role == null) throw new ArgumentNullException("role");

            Context.Roles.Remove(role);

            return Task.FromResult<Object>(null);
        }

        /// <summary>
        /// Finds a role by role id.
        /// </summary>
        /// <param name="roleId">Role id.</param>
        /// <returns></returns>
        public Task<IdentityRole> FindByIdAsync(string roleId)
        {
            var result = Context.Roles.FirstOrDefault(a => a.Id == roleId);

            return Task.FromResult(result);
        }

        /// <summary>
        /// Finds a role by role name.
        /// </summary>
        /// <param name="roleName">Role name to be found.</param>
        /// <returns></returns>
        public Task<IdentityRole> FindByNameAsync(string roleName)
        {
            var result = Context.Roles.FirstOrDefault(a => a.Name == roleName);

            return Task.FromResult(result);
        }

        /// <summary>
        /// Updates a role info.
        /// </summary>
        /// <param name="role">Role to be updated.</param>
        /// <returns></returns>
        public Task UpdateAsync(IdentityRole role)
        {
            if (role == null) throw new ArgumentNullException("role");

            Context.Roles.Update(role);

            return Task.FromResult<Object>(null);
        }

        /// <summary>
        /// Disposes resources.
        /// </summary>
        public virtual void Dispose()
        {
            if (Context != null && contextDisposed)
            {
                Context.Dispose();
                Context = null;
            }
        }
    }
}