using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ManjuuDomain.IDomain;
using ManjuuManagement.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ManjuuManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //让ioc自动帮我注入Filter
            services.AddSingleton<ManjuuExceptionFilter>();

            services.AddMvc(option =>
            {
                var serviceProvider = services.BuildServiceProvider();
                option.Filters.Add(serviceProvider.GetService<ManjuuExceptionFilter>());
                option.ModelBinderProviders.Insert(0, new StringTrimModelBinderProvider(option.InputFormatters));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //配置IOC
            Assembly.Load("ManjuuDomain");
            var infrastructureAssembly = Assembly.Load("ManjuuInfrastructure");
            var repositoryTypes = infrastructureAssembly.GetTypes().Where(p => !p.IsAbstract && typeof(IRepository).IsAssignableFrom(p));
            foreach (var item in repositoryTypes)
            {
                foreach (var itemIntface in item.GetInterfaces())
                {
                    if (typeof(IRepository) == itemIntface) { continue; }
                    services.AddSingleton(itemIntface, item);
                }
            }

            //services.AddDbContext<ManjuuInfrastructure.Repository.Context.HealthManjuuCoreContext>(
            //    opt =>
            //    opt.UseSqlite(Configuration.GetConnectionString("HealthManjuuCore")));





        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        if (context.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                        {
                            context.Response.ContentType = "text/html";
                            await context.Response.SendFileAsync($@"{env.WebRootPath}/manjuuerrors/500.html");
                        }
                    });
                });

                //app.UseStatusCodePagesWithReExecute("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

           
        }
    }
}
