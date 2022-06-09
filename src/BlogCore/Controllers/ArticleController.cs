// 创建人：魏天华 
// 测试添加代码文件头

using BlogCore.Application.UserInfo;
using BlogCore.Application.UserInfo.Reqs;
using BlogCore.Domain.Comm.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace BlogCore.Controllers
{
    public class ArticleController : BlogControllerBase
    {
        private readonly IBloggerAppservice _bloggerAppservice;

        public ArticleController(IBloggerAppservice bloggerAppservice)
        {
            _bloggerAppservice = bloggerAppservice;
        }

        /// <summary>
        /// 获取博客文章详情
        /// </summary>
        /// <param name="req"></param>
        /// <param name="cancellationToken"></param>
        [AllowAnonymous]
        [HttpGet("ArticleAddViews")]
        public async Task<IActionResult> ArticleAddViewsAsync([FromQuery] ArticleAddViewsReq req, CancellationToken cancellationToken)
        {
            var result = await _bloggerAppservice.AddViews(req);
            return Ok(result);
        }
    }
}
