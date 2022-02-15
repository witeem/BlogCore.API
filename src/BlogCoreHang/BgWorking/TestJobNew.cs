using BlogCore.Application.BgWorking;
using System.Threading.Tasks;
using witeem.CoreHelper.DependencyInjection;
using witeem.CoreHelper.Hangfire;
using System;

namespace BlogCoreHang.BgWorking
{
    /// <summary>
    /// 测试定时调度
    /// </summary>
    [SimpleJob(IsOpen = true, JobId = "TestJobNew", CronExpression = "0/12 * * * * ?")]
    public class TestJobNew : BaseRecurringJob
    {
        private readonly ITimedTaskService _timedTaskService;

        public TestJobNew()
        {
            _timedTaskService = IocManager.Instance.GetService<ITimedTaskService>();
        }
        /// <summary>
        /// execute job
        /// </summary>
        /// <returns></returns>
        public override async Task Execute()
        {
            //todo job
            await _timedTaskService.ConsoleTaskAsync("TestJobNew");
        }

        public override void Dispose()
        {

        }
    }
}
