using DevSkill.Blog.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Infrastructure.Data.Seeds
{
    public class RoleSeed
    {
        public static BlogSiteRole[] GetRoles()
        {
            return new BlogSiteRole[]
            {
                new BlogSiteRole
                {
                    Id = new Guid("23fe4e81-5015-42a0-8d76-d1f08c6b227a"),
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "23fe4e81-5015-42a0-8d76-d1f08c6b227a"
                }
            };
        }
    }
}
