// 创建人：魏天华 
// 测试添加代码文件头

using BlogCore.Application.BaseServices.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using witeem.CoreHelper.ExtensionTools.CommonTools;

namespace BlogCore.Authority.Controllers
{
    public class LogsController : BlogControllerBase
    {
        private readonly ILogger<LogsController> _logger;

        public LogsController(ILogger<LogsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 登录授权
        /// </summary>
        [AllowAnonymous]
        [HttpPost("logging")]
        public async Task<IActionResult> Logging([FromBody] LoggingInput input, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Logging: {input.ToJson()}");
            await Task.CompletedTask;
            return Ok(true);
        }
    }
}
