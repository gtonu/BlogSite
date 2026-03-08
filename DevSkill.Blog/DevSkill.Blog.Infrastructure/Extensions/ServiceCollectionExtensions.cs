using DevSkill.Blog.Infrastructure.Identity;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Repositories;
using DevSkill.Blog.Infrastructure.Data;
using DevSkill.Blog.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using DevSkill.Blog.Domain.Utilities;
using DevSkill.Blog.Infrastructure.Utilities;
using DevSkill.Blog.Infrastructure.Identity.Interfaces;

namespace DevSkill.Blog.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDependencyInjections(this IServiceCollection services)
        {
            services.AddScoped<IBlogPostRepository, BlogPostRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IContactUsRepository, ContactUsRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ITermsAndConditionsRepository, TermsAndConditionsRepository>();
            services.AddScoped<IBlogSiteUserRepository, BlogSiteUserRepository>();
            services.AddScoped<IUserReportRepository, UserReportRepository>();
            services.AddScoped<IApplicationUnitOfWork, ApplicationUnitOfWork>();
            services.AddScoped<ApplicationUnitOfWork>();
            services.AddSingleton<IEmailUtility, EmailUtility>();
            services.AddKeyedSingleton<IEmailUtility, HtmlEmailUtility>("Authentication");
        }
        public static void AddApplicationDbContext(this IServiceCollection services,
            string connectionString,Assembly migrationAssembly)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString
            ,(x) => x.MigrationsAssembly(migrationAssembly)));
        }
        public static void AddModifiedIdentity(this IServiceCollection services)
        {
            services
                .AddIdentity<BlogSiteUser, BlogSiteRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserManager<BlogSiteUserManager>()
                .AddRoleManager<BlogSiteRoleManager>()
                .AddSignInManager<BlogSiteSignInManager>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;

                //SignIn settings.
                options.SignIn.RequireConfirmedAccount = true;
            });
        }

        public static void AddPolicy(this IServiceCollection service)
        {
            service.AddAuthorization(options =>
            {
                //category controller policies
                options.AddPolicy("CanViewCategory", policy =>
                {
                    policy.RequireClaim("Category", "Index");
                });
                options.AddPolicy("CanCreateCategory", policy =>
                {
                    policy.RequireClaim("Category", "Create");
                });
                options.AddPolicy("CanEditCategory", policy =>
                {
                    policy.RequireClaim("Category", "Edit");
                });
                options.AddPolicy("CanDeleteCategory", policy =>
                {
                    policy.RequireClaim("Category", "Delete");
                });

                //TagController policies
                options.AddPolicy("CanViewTag", policy =>
                {
                    policy.RequireClaim("Tag", "Index");
                });
                options.AddPolicy("CanCreateTag", policy =>
                {
                    policy.RequireClaim("Tag", "Create");
                });
                options.AddPolicy("CanEditTag", policy =>
                {
                    policy.RequireClaim("Tag", "Edit");
                });
                options.AddPolicy("CanDeleteTag", policy =>
                {
                    policy.RequireClaim("Tag", "Delete");
                });

                //ContactUsController policies
                options.AddPolicy("CanViewContactUs", policy =>
                {
                    policy.RequireClaim("ContactUs", "Index");
                });
                options.AddPolicy("CanEditContactUs", policy =>
                {
                    policy.RequireClaim("ContactUs", "Edit");
                });
                options.AddPolicy("CanReplyContactUs", policy =>
                {
                    policy.RequireClaim("ContactUs", "Reply");
                });
                options.AddPolicy("CanDeleteContactUs", policy =>
                {
                    policy.RequireClaim("ContactUs", "Delete");
                });

                //TermsAndConditionsController policies
                options.AddPolicy("CanViewTermsAndConditons", policy =>
                {
                    policy.RequireClaim("TermsAndConditions", "Index");
                });
                options.AddPolicy("CanCreateTermsAndConditons", policy =>
                {
                    policy.RequireClaim("TermsAndConditions", "Create");
                });
                options.AddPolicy("CanEditTermsAndConditons", policy =>
                {
                    policy.RequireClaim("TermsAndConditions", "Edit");
                });
                options.AddPolicy("CanDeleteTermsAndConditons", policy =>
                {
                    policy.RequireClaim("TermsAndConditions", "Delete");
                });
            });
        }
    }
}
