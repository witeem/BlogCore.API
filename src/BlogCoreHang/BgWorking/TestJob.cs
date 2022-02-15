using BlogCore.Application.BgWorking;
using System;
using System.Threading.Tasks;
using witeem.CoreHelper.DependencyInjection;
using witeem.CoreHelper.Hangfire;

namespace BlogCoreHang.BgWorking
{
    /// <summary>
    /// 测试定时调度
    /// </summary>
    [SimpleJob(IsOpen = true, JobId = "TestJob", CronExpression = "0/5 * * * * ?")]
    public class TestJob : BaseRecurringJob
    {
        private readonly ITimedTaskService _timedTaskService;

        public TestJob()
        {
            _timedTaskService =IocManager.Instance.GetService<ITimedTaskService>();
        }

        /// <summary>
        /// execute job
        /// </summary>
        /// <returns></returns>
        public override async Task Execute()
        {
            //todo job
             _timedTaskService.ConsoleTask("TestJob");
            await Task.Delay(1000);
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);//通知垃圾回收器不用再调用终结器
        }
    }
}
