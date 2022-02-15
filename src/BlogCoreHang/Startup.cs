using Autofac;
using Autofac.Extensions.DependencyInjection;
using BlogCore.Application.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using witeem.CoreHelper.DependencyInjection;
using witeem.CoreHelper.ExtensionTools;
using witeem.CoreHelper.Hangfire;
using witeem.CoreHelper.HttpHelper;

namespace BlogCoreHang
{
    public class Startup
    {

        public static readonly ILoggerFactory EFLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
        public Startup(IWebHostEnvironment env)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();
            MyEnvironment = env;
            ConfigManagerHelper.SetConfiguration(Configuration);
        }

        public IConfigurationRoot Configuration { get; private set; }

        public ILifetimeScope AutofacContainer { get; private set; }

        public IWebHostEnvironment MyEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddControllers();
            services.AddHttpHelperService();
            services.AddIocManager();

            #region EF Core
            /*
            //注入EF Core数据库上下文服务
            services.AddDbContext<DefaultContext>(options =>
            {
                var appSetting = ConfigManagerHelper.GetSection<AppSetting>(BlogCoreConsts.AppSetting);
                AESEncryptOut encryptOut = new AESEncryptOut()
                {
                    Content = Configuration.GetConnectionString(BlogCoreConsts.Default),
                    Key = appSetting.ConnKey,
                    Iv = appSetting.ConnIV
                };

                string connStr = EncyptHelper.AESDecrypt(encryptOut.Content, encryptOut.Key, encryptOut.Iv);
                options.UseMySQL(connStr);
                if (_hostEnvironment.IsDevelopment())
                {
                    //打印sql
                    options.UseLoggerFactory(EFLoggerFactory);
                    options.EnableSensitiveDataLogging(true);//显示sql参数
                }
            });
            */
            #endregion

            #region SqlSugar
            services.AddSqlSugarSetup();
            #endregion

            //注入Hangfire服务
            services.AddWiteemHangfire(Configuration);

            // 禁用端点路由
            services.AddMvc(options => options.EnableEndpointRouting = false);
        }


        //您可以在ConfigureContainer中直接注册内容
        //使用Autofac。 这在ConfigureServices之后运行
        //这里将覆盖在ConfigureServices中进行的注册。
        //不要建造容器; 为你做完了。 如果你
        //需要对容器的引用，你需要使用
        //后面会显示“没有ConfigureContainer”机制。
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // 在此处直接向Autofac注册您自己的东西。不要调用为您在AutofacServiceProviderFactory中发生的builder.Populate（）。
            builder.RegisterModule(new ServiceModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseRouting();
            #region Hangfire
            app.UseIocManager();
            app.UseWiteemHangfire();
            #endregion

            // 返回错误码
            app.UseStatusCodePages();//把错误码返回前台，比如是404
            app.UseMvc();

        }
    }
}
