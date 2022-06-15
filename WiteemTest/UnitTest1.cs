using BlogCore.Core.Models;
using Consul;
using RestSharp;
using System;
using System.Linq;
using witeem.CoreHelper.DependencyInjection;
using Xunit;

namespace WiteemTest
{
    public class UnitTest1
    {
        [Fact]
        public void CreateDataBase()
        {
            DataBaseOperate.CreateTable();
        }

        [Fact]
        public void ClearConsulServers()
        {
            var consuleClient = new ConsulClient(c => c.Address = new Uri("http://134.175.26.46:8500"));
            var services = consuleClient.Agent.Services().Result.Response;
            var deregisterList = services.Values.Where(m => m.Service == "service-blog-api").ToList();
            int deregisterCount = deregisterList.Count;
            foreach (var item in deregisterList)
            {
                if (deregisterCount > 1)
                {
                    if (item.Service.Contains("service-blog-api"))
                    {
                        consuleClient.Agent.ServiceDeregister(item.ID);
                    }

                    deregisterCount--;
                }
            }
        }
    }
}