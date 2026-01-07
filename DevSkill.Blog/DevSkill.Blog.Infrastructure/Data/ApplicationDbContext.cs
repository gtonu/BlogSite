using Demo.Infrastructure.Identity;
using DevSkill.Blog.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Blog.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<BlogSiteUser,
        BlogSiteRole,
        Guid,
        BlogSiteUserClaim,
        BlogSiteUserRole,
        BlogSiteUserLogin,
        BlogSiteRoleClaim,
        BlogSiteUserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<BlogPost> BlogPosts { get; set; }
    }
}
