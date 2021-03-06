using Autofac;
using Autofac.Extensions.DependencyInjection;
using BlogCore.Application;
using BlogCore.Application.Exceptions;
using BlogCore.Application.MyMiddleware;
using BlogCore.Core;
using BlogCore.Core.CommonHelper.Helper;
using BlogCore.Core.Models;
using BlogCore.Filter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;
using witeem.CoreHelper.ExtensionTools;
using witeem.CoreHelper.ExtensionTools.CommonTools;
using witeem.CoreHelper.ExtensionTools.Dtos;
using witeem.CoreHelper.HttpHelper;
using witeem.CoreHelper.Redis;
using witeem.CoreHelper.Swagger;

namespace BlogCore
{
    public class Startup
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        /// <summary>
        /// 配置文件
        /// </summary>
        public IConfiguration Configuration { get; private set; }

        /// <summary>
        /// AotoFac容器
        /// </summary>
        public ILifetimeScope AutofacContainer { get; private set; }

        /// <summary>
        /// EF打印SQL
        /// </summary>
        public static readonly ILoggerFactory EFLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _hostEnvironment = env;
            ConfigManagerHelper.SetConfiguration(Configuration);
        }

        //在ConfigureServices中注册依赖项。
        //在下面的ConfigureContainer方法之前由运行时调用。
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddControllers();
            services.AddHttpHelperService();
            services.AddMediatR(typeof(Startup));
            services.AddMediatR(typeof(BaseHandler));
            services.AddHealthChecks();

            services.AddSingleton(new LogHelper(_hostEnvironment.ContentRootPath));
            services.Configure<AppSetting>(Configuration.GetSection(BlogCoreConsts.AppSetting));
            services.Configure<JwtSettings>(Configuration.GetSection(BlogCoreConsts.JwtSettings));

            // SqlSugar
            services.AddSqlSugarSetup();

            // AutoMap
            // 参数类型是Assembly类型的数组 表示AutoMapper将在这些程序集数组里面遍历寻找所有继承了Profile类的配置文件
            // 在当前作用域的所有程序集里面扫描AutoMapper的配置文件
            // services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(Assembly.Load("BlogCore.Application"));
            if (_hostEnvironment.IsDevelopment())
            {
                // Swagger
                services.AddWiteemSwagger(Configuration, _hostEnvironment.ContentRootPath);
            }

            // Redis
            services.AddWiteemRedis(Configuration);

            //添加全局权限认证过滤器
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<ApiAuthorizeAttribute>();
            });

            //禁用端点路由
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

        //配置是添加中间件的位置。 这称之为
        // ConfigureContainer。 您可以使用IApplicationBuilder.ApplicationServices
        //如果你需要从容器中解决问题。
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IHostApplicationLifetime lifetime)
        {
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();
            if (env.IsDevelopment())
            {
                // 在开发环境中，使用异常页面，这样可以暴露错误堆栈信息，所以不要放在生产环境。
                app.UseDeveloperExceptionPage();

                #region Swagger
                app.UseWiteemSwagger();
                #endregion
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

#if !DEBUG
            // 注册服务发现
            app.RegisterConsul(lifetime);
#endif
            // 跳转https
            app.UseHttpsRedirection();
            //启用jwt认证中间件
            app.UseMiddleware<JwtMiddleware>();
            //启用签名认证中间件
            app.UseMiddleware<SignMiddleware>();
            // 返回错误码
            app.UseStatusCodePages();//把错误码返回前台，比如是404
            app.UseRouting();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/healthz");
            });
        }
    }
}
