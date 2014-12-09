using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using System;
using ServiceStack.DataAnnotations;

namespace Kesoft.AspNet.Identity.MySql
{
    /// <summary>
    /// Class that implements the ASP.NET Identity IUser interface.
    /// </summary>
    public class IdentityUser : IUser
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public IdentityUser()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Constructor that takes user name as argument.
        /// </summary>
        /// <param name="userName">User name.</param>
        public IdentityUser(string userName)
            : this()
        {
            UserName = userName;
        }

        /// <summary>
        /// User ID.
        /// </summary>
        [Key]
        [StringLength(50)]
        public string Id { get; set; }

        /// <summary>
        /// User's name.
        /// </summary>
        [StringLength(50)]
        [Index]
        public string UserName { get; set; }

        /// <summary>
        /// User's password hash.
        /// </summary>
        [StringLength(100)]
        public string PasswordHash { get; set; }

        /// <summary>
        /// User's security stamp.
        /// </summary>
        [StringLength(50)]
        public string SecurityStamp { get; set; }

        /// <summary>
        /// User's real name.
        /// </summary>
        [StringLength(32)]
        [Index]
        public string RealName { get; set; }

        /// <summary>
        /// Register time.
        /// </summary>
        [Index]
        public DateTime RegisterTime { get; set; }

        /// <summary>
        /// Email address.
        /// </summary>
        [StringLength(50)]
        public string Email { get; set; }
    }
}
