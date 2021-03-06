﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
//using Microsoft.Framework.DependencyInjection;
using TheWorld.Services;
using Microsoft.Extensions.PlatformAbstractions;
using TheWorld.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNet.Mvc.Formatters.Xml;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Formatters;
using Newtonsoft.Json.Serialization;
using TheWorld.Controllers.Api;
using Microsoft.AspNet.Authentication.Cookies;
using System.Net;

namespace TheWorld
{
    public class Startup
    {
        public static IConfigurationRoot configuration;

        public Startup(IApplicationEnvironment appEnv)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();
            

            configuration = builder.Build();
                
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc( config =>
            {
#if !DEBUG
                config.Filters.Add(new RequireHttpsAttribute());
#endif

            })



                .AddXmlDataContractSerializerFormatters()
                .AddJsonOptions(opt => {
                    opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
                
#if DEBUG
            services.AddScoped<IMailService, DebugMailService>() ;
#else
             //services.AddScoped<IMailService, MailService>() ;
#endif
            services.AddIdentity<WorldUser, Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 8;
                config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
                config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = ctx=>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api") &&
                            ctx.Response.StatusCode==(int)HttpStatusCode.OK)
                        {
                            ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        }
                        else
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }
                        
                        return Task.FromResult(0);
                    }
                };
            })
            .AddEntityFrameworkStores<WorldContext>();
            services.AddLogging();
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<WorldContext>();

            // services.Configure<MvcOptions>(options =>
            //{
            ////options.AddXmlDataContractSerializerFormatter());
            //     options.InputFormatters.Add(new XmlSerializerInputFormatter());
            //     options.OutputFormatters.Add(new XmlSerializerOutputFormatter());

            //});

            services.AddScoped<CoordService>();
            services.AddTransient<WorldContextSeedData>() ;

            services.AddScoped<IWorldRepository, WorldRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, WorldContextSeedData seeder, ILoggerFactory loggerFactory )
            
        {

            loggerFactory.AddDebug(LogLevel.Warning);


            //app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseIdentity();    /// use user identity
            AutoMapper.Mapper.Initialize(config =>
            {
                config.CreateMap<Trip, TripViewModel>().ReverseMap();
                config.CreateMap<Stop, StopViewModel>().ReverseMap();
            });


            app.UseMvc( config =>
            {
                config.MapRoute(name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "App", action = "Index" });

            });
            await seeder.EnsureSeedDataAsync();

            

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync($"Hello World!{context.Request.Path}");
            //});
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
