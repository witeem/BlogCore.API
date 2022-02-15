using Autofac;
using BlogCore.Application;
using BlogCore.Domain;

namespace BlogCore
{
    public class ServiceModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new ApplicationModule());
            builder.RegisterModule(new DomainModule());
            builder.RegisterModule(new RepositoryModule());

            //Core层实现接口的类自动依赖注入
            var assemblysCore = System.Reflection.Assembly.Load("BlogCore.Core");
            builder.RegisterAssemblyTypes(assemblysCore)
                .AsImplementedInterfaces();

            // 仓储注入
            // builder.RegisterGeneric(typeof(RepositoryBase<,>)).As(typeof(IRepositoryBase<,>)).InstancePerDependency();
            // builder.RegisterGeneric(typeof(RepositoryBase<>)).As(typeof(IRepositoryBase<>)).InstancePerDependency();
        }
    }
}
