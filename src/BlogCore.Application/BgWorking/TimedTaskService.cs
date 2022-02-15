using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlogCore.Application.BgWorking
{
    public class TimedTaskService : ITimedTaskService
    {
        private readonly ILogger<TimedTaskService> _logger;
        public TimedTaskService(ILogger<TimedTaskService> logger) 
        {
            _logger = logger;
        }

        public async Task ConsoleTaskAsync(string taskName)
        {
            _logger.LogInformation($"{DateTime.Now}: 正在输出【{taskName}】");
            await Task.Delay(1000);
        }

        public void ConsoleTask(string taskName)
        {
            _logger.LogInformation($"{DateTime.Now}: 正在输出【{taskName}】");
        }
    }
}
