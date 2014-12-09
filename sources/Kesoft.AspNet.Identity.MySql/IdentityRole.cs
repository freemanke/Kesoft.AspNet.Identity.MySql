using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using System;
using ServiceStack.DataAnnotations;

namespace Kesoft.AspNet.Identity.MySql
{
    /// <summary>
    /// Class that implements the ASP.NET Identity IRole interface.
    /// </summary>
    public class IdentityRole : IRole
    {
        /// <summary>
        /// Default constructor for Role. 
        /// </summary>
        public IdentityRole()
        {
            Id = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// Constructor that takes names as argument.
        /// </summary>
        /// <param name="name">Role name.</param>
        public IdentityRole(string name) : this()
        {
            Name = name;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Role name.</param>
        /// <param name="id">Role id.</param>
        public IdentityRole(string name, string id)
        {
            Name = name;
            Id = id;
        }

        /// <summary>
        /// Role ID.
        /// </summary>
        [Key]
        [StringLength(50)]
        public string Id { get; set; }

        /// <summary>
        /// Role name.
        /// </summary>
        [StringLength(50)]
        [Index]
        public string Name { get; set; }
    }
}
