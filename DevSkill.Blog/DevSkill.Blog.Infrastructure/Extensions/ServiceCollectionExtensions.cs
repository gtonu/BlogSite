using Demo.Infrastructure.Identity;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Repositories;
using DevSkill.Blog.Infrastructure.Data;
using DevSkill.Blog.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDependencyInjections(this IServiceCollection services)
        {
            services.AddScoped<IBlogPostRepository, BlogPostRepository>();
            services.AddScoped<IApplicationUnitOfWork, ApplicationUnitOfWork>();
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
            });
        }
    }
}
