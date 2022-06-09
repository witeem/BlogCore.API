// 创建人：魏天华 
// 测试添加代码文件头

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlogCore.Authority.Controllers
{
    public class LogsController : BlogControllerBase
    {
        /// <summary>
        /// 登录授权
        /// </summary>
        [AllowAnonymous]
        [HttpGet("health")]
        public async Task<IActionResult> health()
        {
            await Task.CompletedTask;
            return Ok(true);
        }
    }
}
