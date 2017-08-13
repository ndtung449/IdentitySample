namespace IdentitySample
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using IdentitySample.Data;
    using IdentitySample.Models;
    using IdentitySample.Services;
    using IdentitySample.DataMapper;
    using IdentitySample.Data.Services;
    using IdentitySample.Data.UnitOfWork;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Rewrite;
    using Microsoft.AspNetCore.Authorization;
    using IdentitySample.Authorization;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using System;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
            AutoMapperConfiguration.Config();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            // Require SSL for all requests
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });

            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            // Config identity
            //services.Configure<IdentityOptions>(options =>
            //{
            //    //options.Password.RequireDigit = false;
            //    //options.Password.RequiredLength = 6;
            //    //options.Password.RequireLowercase = false;
            //    //options.Password.RequireNonAlphanumeric = false;
            //    //options.Password.RequireUppercase = false;

            //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            //    options.Lockout.MaxFailedAccessAttempts = 5;

            //    options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(90);
            //    options.Cookies.ApplicationCookie.LoginPath = "/Account/Login";
            //    options.Cookies.ApplicationCookie.LogoutPath = "/Account/Logout";

            //    options.User.RequireUniqueEmail = true;

            //    options.SignIn.RequireConfirmedEmail = true;
            //});

            // Authorization policies
            services.AddAuthorization(config =>
            {
                config.AddPolicy("RequireAdminOrUserModPolicy",
                    policy => policy.RequireRole(Constants.AdminRole, Constants.UserModRole));
                config.AddPolicy("AdminOnly",
                    policy => policy.RequireRole(Constants.AdminRole));
            });

            services.Configure<SendingEmailOptions>(Configuration.GetSection("SendingEmailOptions"));

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Add authorization handlers
            services.AddSingleton<IAuthorizationHandler, AdminAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, UserModAuthorizationHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            ApplicationDbContext dbContext)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // Redirect to Https
            var rewriteOptions = new RewriteOptions().AddRedirectToHttps();
            app.UseRewriter(rewriteOptions);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            app.UseFacebookAuthentication(new FacebookOptions
            {
                AppId = Configuration["Authentication:Facebook:AppID"],
                AppSecret = Configuration["Authentication:Facebook:AppSecret"]
            });

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Article}/{action=Index}/{id?}");
            });

            var testPassword = Configuration["SeedPassword"];
            DbInitializer.Initialize(app.ApplicationServices, testPassword).Wait();
        }
    }
}
