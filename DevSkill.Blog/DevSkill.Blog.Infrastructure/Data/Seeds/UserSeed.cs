using DevSkill.Blog.Infrastructure.Data.Migrations;
using DevSkill.Blog.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Infrastructure.Data.Seeds
{
    public class UserSeed
    {
        public static BlogSiteUser[] GetUsers()
        {
            return new BlogSiteUser[]
            {
                new BlogSiteUser
                {
                    Id = new Guid("605fbeb7-f2bc-4d2c-886d-08de6c1b8c01"),
                    UserName = "Admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@gmail.com",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAIAAYagAAAAEFF00oytfTeMeV0LotTISYwoFEu+ngUmsBx4nHUnRh7xSdc1e9aVj6p4VGmRY4HWZg==",
                    SecurityStamp = "GDOC34LIOZXWECAJBPLLACWMF2CBR6GH",
                    ConcurrencyStamp = "bfb063fb-c852-4248-bd51-6c8e56f3e1bf"
                }
            };
        }
    }
}
