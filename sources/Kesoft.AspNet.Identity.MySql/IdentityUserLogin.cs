using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.OrmLite;

namespace Kesoft.AspNet.Identity.MySql
{
    public class IdentityUserLogin
    {
        [Key]
        [StringLength(50)]
        [ForeignKey(typeof (IdentityUser), OnDelete = "cascade")]
        public string UserId { get; set; }

        [Key]
        [StringLength(100)]
        public string LoginProvider { get; set; }

        [Key]
        [StringLength(100)]
        public string ProviderKey { get; set; }
    }
}
