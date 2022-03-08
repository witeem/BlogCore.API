using BlogCore.Application.AdverUserHandler.Model;
using BlogCore.Application.BloggerHandler.Model;
using BlogCore.Domain.Comm.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using witeem.CoreHelper.ExtensionTools.CommonTools;

namespace BlogCore.Controllers
{
    public class AdverUserController : BlogControllerBase
    {
        private readonly IMediator _mediator;
        public AdverUserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetUserInfo")]
        public async Task<IActionResult> GetUserInfoAsync([FromBody] AdverUserInfoModel req, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(req);
            return Ok(ApiResponce<object>.Success(result));
        }

        /// <summary>
        /// 获取博主信息
        /// </summary>
        /// <param name="req">req</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetBloggerInfo")]
        public async Task<IActionResult> GetBloggerInfoAsync([FromQuery] BloggerInfoModel req, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(req);
            return Ok(ApiResponce<object>.Success(result));  
        }
    }
}
