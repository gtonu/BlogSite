using Cortex.Mediator.DependencyInjection;
using DevSkill.Blog.Application.Features.Post.Commands;
using DevSkill.Blog.Domain.Email;
using DevSkill.Blog.Infrastructure;
using DevSkill.Blog.Infrastructure.Data;
using DevSkill.Blog.Infrastructure.Extensions;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;

//Bootstrap logger configuration
#region bootstrap_logger
Log.Logger = new LoggerConfiguration()
                 .WriteTo.File("Logs/log_book-.log",rollingInterval:RollingInterval.Day)
                 .CreateBootstrapLogger();
#endregion

try
{
   
    var builder = WebApplication.CreateBuilder(args);
    var googleClientId = builder.Configuration["web:client_id"];
    var googleClientSecret = builder.Configuration["web:client_secret"];
    var githubClientId = builder.Configuration["github:client_id"];
    var githubClientSecret = builder.Configuration["github:client_secret"];

    // Adding services/dependencies to the built-in dependency injection container of ASP.NET framework.(Service collection)
    #region Adding dependencies to DI container
    //general logger configuration
    #region Serilog Configuration
    builder.Host.UseSerilog((context, lc) => lc
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(builder.Configuration)
        );
    #endregion

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
        throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    var migrationAssembly = Assembly.GetAssembly(typeof(ApplicationDbContext));

    #region google login
    builder.Services.AddAuthentication().AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = googleClientId;
        googleOptions.ClientSecret = googleClientSecret;
    });
    #endregion

    #region Github Login
    builder.Services.AddAuthentication().AddGitHub(options =>
    {
        options.ClientId = githubClientId;
        options.ClientSecret = githubClientSecret;
    });
    #endregion

    #region Service collection based dependency injection
    builder.Services.AddDependencyInjections();
    #endregion

    #region Cortex mediator configuration
    builder.Services.AddCortexMediator(
        builder.Configuration,
        new[] { typeof(Program), typeof(CreateBlogPostCommand) },
        options => options.AddDefaultBehaviors()
        );
    #endregion

    #region Mapster configuration

    //var config = TypeAdapterConfig.GlobalSettings;
    //config.Scan(typeof(MapsterConfiguration).Assembly);
    //builder.Services.AddSingleton(config);
    //builder.Services.AddScoped<IMapper,ServiceMapper>();

    //Default configuration
    builder.Services.AddMapster();
    #endregion

    #region ApplicationDbcontext binding
    builder.Services.AddApplicationDbContext(connectionString, migrationAssembly);
    #endregion

    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    #region Identity Framework configuration
    builder.Services.AddModifiedIdentity();
    #endregion

    #region Mapping Mailtrap configuration with SmtpSettings class from appsettings.json
    builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
    #endregion

    builder.Services.AddControllersWithViews();
    builder.Services.AddRazorPages();
    #endregion

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseRouting();

    app.UseAuthorization();

    app.MapStaticAssets();

    app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();

    app.MapRazorPages()
       .WithStaticAssets();

    app.Run();
}
catch (Exception ex){
    Log.Fatal(ex, "Application crashed due to a fatal error!");
}
finally
{
    Log.CloseAndFlush();
}
