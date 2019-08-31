using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using JKang.IpcServiceFramework;
using ManjuuApplications;
using ManjuuCommon.ILog;
using ManjuuDomain.IDomain;
using ManjuuInfrastructure.IpcService.ServiceContract;
using ManjuuManagement.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;

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



            //配置IOC
            //Assembly.Load("ManjuuDomain");
            #region IRepository
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
            #endregion

            #region ICustomLog
            var commonAssembly = Assembly.Load("ManjuuCommon");
            var customLogTypes = commonAssembly.GetTypes().Where(p => !p.IsAbstract && typeof(ICustomLog<ILogger>).IsAssignableFrom(p));
            foreach (var item in customLogTypes)
            {
                foreach (var itemIntface in item.GetInterfaces())
                {
                    if (typeof(ICustomLog<ILogger>) == itemIntface) { continue; }
                    services.AddSingleton(itemIntface, item);
                }
            }
            #endregion

            #region IApplication
            var applicationAssembly = Assembly.Load("ManjuuApplications");
            var applicationTypes = applicationAssembly.GetTypes().Where(p => !p.IsAbstract && typeof(IApplication).IsAssignableFrom(p));
            foreach (var item in applicationTypes)
            {
                foreach (var itemIntface in item.GetInterfaces())
                {
                    if (typeof(IApplication) == itemIntface) { continue; }
                    //同一个请求下单例
                    services.AddScoped(itemIntface, item);
                }
            }
            #endregion

            #region Ipc
            //services.AddIpc(action =>
            //{
            //    action.AddNamedPipe(pipeOpt =>
            //    {
            //        pipeOpt.ThreadCount = 2;
            //    }).AddService<IDemoServiceContract, DemoServiceContract>();

            //});
            #endregion

            #region Ipc client
            //IpcServiceClient<IDemoServiceContract> client = new IpcServiceClientBuilder<IDemoServiceContract>()
            //.UseNamedPipe("pipeName") // or .UseTcp(IPAddress.Loopback, 45684) to invoke using TCP 
            //.Build();
            IpcServiceClient<ICheckConfigServiceContract> configClient = new IpcServiceClientBuilder<ICheckConfigServiceContract>()
           .UseNamedPipe("configPipe")
           .Build();

            IpcServiceClient<ICheckTargetServiceContract> targetJobClient = new IpcServiceClientBuilder<ICheckTargetServiceContract>()
        .UseNamedPipe("targetPipe")
        .Build();

            services.AddSingleton<IpcServiceClient<ICheckConfigServiceContract>>(configClient);
            services.AddSingleton<IpcServiceClient<ICheckTargetServiceContract>>(targetJobClient);
            #endregion

            //让ioc自动帮我注入Filter
            services.AddSingleton<ManjuuExceptionFilter>();


            //services.AddDbContext<ManjuuInfrastructure.Repository.Context.HealthManjuuCoreContext>(
            //    opt =>
            //    opt.UseSqlite(Configuration.GetConnectionString("HealthManjuuCore")));



            services.AddMvc(option =>
            {
                var serviceProvider = services.BuildServiceProvider();
                option.Filters.Add(serviceProvider.GetService<ManjuuExceptionFilter>());
                option.ModelBinderProviders.Insert(0, new StringTrimModelBinderProvider(option.InputFormatters));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            //var ipcServerHostBuilder = new IpcServiceHostBuilder(serviceProvider)
            //    .AddNamedPipeEndpoint<IDemoServiceContract>("demoEnpoint", "pipeName")
            //    .AddTcpEndpoint<IDemoServiceContract>("demoTcpEndpoiint",IPAddress.Loopback,2324)
            //    .Build()
            //    .RunAsync();

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
