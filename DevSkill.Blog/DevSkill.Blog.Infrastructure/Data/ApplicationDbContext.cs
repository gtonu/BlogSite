using DevSkill.Blog.Infrastructure.Identity;
using DevSkill.Blog.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using DevSkill.Blog.Infrastructure.Data.Seeds;

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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BlogSiteRole>().HasData(RoleSeed.GetRoles());
            builder.Entity<BlogSiteUser>().HasData(UserSeed.GetUsers());
            builder.Entity<BlogSiteUserRole>().HasData(UserRoleSeed.GetUserRoles());
            builder.Entity<Category>().HasData(CategorySeed.GetCategories());
            builder.Entity<Tag>().HasData(TagSeed.GetTags());
            builder.Entity<BlogPostCategory>().ToTable("BlogPostCategories");
            builder.Entity<BlogPostTag>().ToTable("BlogPostTags");
            builder.Entity<BlogPostCategory>().HasKey(x => new { x.BlogPostId, x.CategoryId });
            builder.Entity<BlogPostTag>().HasKey(x => new { x.BlogPostId, x.TagId });


            //one to many relationships
            builder.Entity<BlogPost>()
                .HasMany(x => x.Comments)
                .WithOne(y => y.Post)
                .HasForeignKey(z => z.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            //one to many self-referencig relation
            builder.Entity<Comment>()
                .HasMany(x => x.Replies)
                .WithOne(y => y.ParentComment)
                .HasForeignKey(z => z.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);

            //many to many relationships
            builder.Entity<BlogPostCategory>()
                .HasOne(x => x.BlogPost)
                .WithMany(y => y.Categories)
                .HasForeignKey(z => z.BlogPostId);
            builder.Entity<BlogPostCategory>()
                .HasOne(x => x.Category)
                .WithMany(y => y.BlogPosts)
                .HasForeignKey(z => z.CategoryId);

            builder.Entity<BlogPostTag>()
                .HasOne(x => x.BlogPost)
                .WithMany(y => y.Tags)
                .HasForeignKey(z => z.BlogPostId);
            builder.Entity<BlogPostTag>()
                .HasOne(x => x.Tag)
                .WithMany(y => y.BlogPosts)
                .HasForeignKey(z => z.TagId);

            base.OnModelCreating(builder);
        }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TermsAndConditions> TermsAndConditions { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<UserReport> UserReports { get; set; }
    }
}
