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
    public class IdentityUserClaim
    {
        [Key]
        [AutoIncrement]
        public int Id { get; set; }

        [StringLength(50)]
        [ForeignKey(typeof(IdentityUser), OnDelete = "cascade")]
        public string UserId { get; set; }

        [StringLength(100)]
        public string ClaimType { get; set; }

        [StringLength(100)]
        public string ClaimValue { get; set; }
    }
}
