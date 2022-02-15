using Autofac.Extras.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogCore.Application
{
    [Intercept(typeof(ServiceInterceptor))]
    public interface IAppService
    {

    }
}
