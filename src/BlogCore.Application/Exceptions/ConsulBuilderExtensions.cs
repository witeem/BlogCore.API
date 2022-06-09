// 创建人：魏天华 
// 测试添加代码文件头

using BlogCore.Core.ComSettings;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using witeem.CoreHelper.ExtensionTools;

namespace BlogCore.Application.Exceptions
{
    public static class ConsulBuilderExtensions
    {
        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime)
        {
            ConsulOption consulOption = ConfigManagerHelper.GetSection<ConsulOption>("Consul");
            if (string.IsNullOrWhiteSpace(consulOption?.ServiceName) || string.IsNullOrWhiteSpace(consulOption?.Address))
            {
                throw new ArgumentNullException("ServiceDiscovery.ServiceName/Consul.Address cannot be null or empty!");
            }

            var consulClient = new ConsulClient(x =>
            {
                x.Address = new Uri(consulOption.Address);
            });

            IDictionary<string, string> metas = new Dictionary<string, string>();
            metas.Add("Weight", consulOption.Weight ?? "1"); // 权重
            var registration = new AgentServiceRegistration()
            {
                ID = Guid.NewGuid().ToString(),
                Name = consulOption.ServiceName,// 服务名
                Address = consulOption.ServiceIP, // 服务绑定IP
                Port = consulOption.ServicePort, // 服务绑定端口
                Meta = metas,
                Check = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                    Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔
                    HTTP = consulOption.ServiceHealthCheck + "/healthz",//健康检查地址
                    Timeout = TimeSpan.FromSeconds(5)
                }
            };

            // 服务注册
            consulClient.Agent.ServiceRegister(registration).Wait();

            // 应用程序终止时，服务取消注册
            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            });
            return app;
        }
    }
}
