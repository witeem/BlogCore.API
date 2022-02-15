using BlogCore.Application.AdverUserHandler.Model;
using BlogCore.Application.UserInfo;
using BlogCore.Application.UserInfo.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            return Ok(await _mediator.Send(req));
        }

    }
}
