using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BlogCore.Application
{
    /// <summary>
    ///  AOP 功能
    /// </summary>
    public class ServiceInterceptor : IInterceptor
    {
        public virtual void Intercept(IInvocation invocation)
        {
            Console.WriteLine($"{DateTime.Now}: 方法当前函数：{invocation.Method.Name}");
            invocation.Proceed();
        }
    }
}
