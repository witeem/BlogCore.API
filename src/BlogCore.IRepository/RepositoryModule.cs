// 创建人：魏天华 
// 测试添加代码文件头

using Autofac;
using BlogCore.Core.ModulesInterface;

public class RepositoryModule : Module
{
    public RepositoryModule()
    {

    }
    protected override void Load(ContainerBuilder builder)
    {
        //注册服务
        var iRepository = System.Reflection.Assembly.Load("BlogCore.IRepository");
        var repository = System.Reflection.Assembly.Load("BlogCore.Repository");
        builder.RegisterAssemblyTypes(new System.Reflection.Assembly[] { iRepository, repository })
            .Where(t => t.IsAssignableTo<IRepositoryCore>())
            //根据类型注册组件IAppService,暴漏其实现的所有服务（接口）
            .AsImplementedInterfaces()
            //每个生命周期作用域一个实例
            .InstancePerLifetimeScope();
        ////动态注入拦截器
        //.EnableInterfaceInterceptors()
        //.InterceptedBy(typeof(OpsLogInterceptor), typeof(EasyCachingInterceptor));
        //builder.RegisterType(typeof(AdvertisementDomainServices)).As<IAdvertisementDomainServices>().InstancePerLifetimeScope();
    }
}
