using DevSkill.Blog.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Infrastructure.Data.Seeds
{
    public class UserRoleSeed
    {
        public static BlogSiteUserRole[] GetUserRoles()
        {
            return new BlogSiteUserRole[]
            {
                new BlogSiteUserRole
                {
                    RoleId = new Guid("23fe4e81-5015-42a0-8d76-d1f08c6b227a"),
                    UserId = new Guid("605fbeb7-f2bc-4d2c-886d-08de6c1b8c01")
                }
            };
        }
    }
}
