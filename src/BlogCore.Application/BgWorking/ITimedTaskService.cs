using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Application.BgWorking
{
    public interface ITimedTaskService : IAppService
    {
        Task ConsoleTaskAsync(string taskName);

        void ConsoleTask(string taskName);
    }
}
