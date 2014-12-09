using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

namespace Kesoft.AspNet.Identity.MySql
{
    public class IdentityUserInRole
    {
        [Key]
        [AutoIncrement]
        public int Id { get; set; }

        [StringLength(50)]
        [ForeignKey(typeof (IdentityUser), OnDelete = "cascade", OnUpdate = "cascade")]
        public string UserId { get; set; }

        [StringLength(50)]
        [ForeignKey(typeof (IdentityRole), OnDelete = "cascade", OnUpdate = "cascade")]
        public string RoleId { get; set; }
    }
}
