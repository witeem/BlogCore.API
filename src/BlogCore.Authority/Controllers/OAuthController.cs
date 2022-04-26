using BlogCore.Application.SignInfo;
using BlogCore.Application.UserInfo;
using BlogCore.Application.UserInfo.Dtos;
using BlogCore.Domain.Comm.Dto;
using BlogCore.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using witeem.CoreHelper.ExtensionTools.CommonTools;

namespace BlogCore.Authority.Controllers
{
    /// <summary>
    /// SSO 授权
    /// </summary>
    public class OAuthController : BlogControllerBase
    {
        private readonly IUserInfoAppService _userInfoAppService;
        private readonly ISignInfoAppService _signInfoAppService;

        /// <summary>
        /// SSO 授权
        /// </summary>
        /// <param name="userInfoAppService"></param>
        /// <param name="signInfoAppService"></param>
        public OAuthController(IUserInfoAppService userInfoAppService, ISignInfoAppService signInfoAppService) : base()
        {
            _userInfoAppService = userInfoAppService;
            _signInfoAppService = signInfoAppService;
        }

        /// <summary>
        /// 登录授权
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("Login")]
        public async Task<IActionResult> Login([FromQuery] UserSearchDto userDto)
        {
            try
            {
                var user = await _userInfoAppService.GetUserInfoAsync(userDto);
                if (user == null) return Unauthorized();
                #region 生成token令牌
                var tokenString = await _userInfoAppService.GetJwtToken(user.Data);
                #endregion

                return Ok(ApiResponce<object>.Success(new
                {
                    success = true,
                    access_token = tokenString.Data,
                    token_type = "Bearer"
                }));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponce<object>.Fail((int)ApiResponceEnum.Error, $"请求失败, {ex.Message}"));
            }
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        [HttpGet("LogOut")]
        public async Task<IActionResult> LogOut()
        {
            var result = await _userInfoAppService.LogOut();
            if (result.Data)
            {
                Response.Cookies.Delete("sso-token");
            }
            return Ok(result);
        }

        /// <summary>
        /// 获取当前用户 (登录状态检测)
        /// </summary>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns></returns>
        [HttpGet("GetAuthenticate")]
        public async Task<IActionResult> GetAuthenticate(CancellationToken cancellationToken)
        {
            return Ok(await _userInfoAppService.GetAuthenticate());
        }

        /// <summary>
        /// 新增用户信息
        /// </summary>
        /// <param name="req">req</param>
        /// <param name="cancellationToken">cancellationToken</param>
        [AllowAnonymous]
        [HttpPost("AddUserInfo")]
        public async Task<IActionResult> AddUserInfoAsync([FromBody] AdverUserInfoDto req, CancellationToken cancellationToken)
        {
            return Ok(await _userInfoAppService.AddUserInfoAsync(req));
        }

        /// <summary>
        /// 获取当前用户所属菜单
        /// </summary>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns></returns>
        [HttpGet("GetMenus")]
        public async Task<IActionResult> GetMenus(CancellationToken cancellationToken)
        {
            return Ok(await _userInfoAppService.GetMenus());
        }

        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <param name="input">input</param>
        /// <returns></returns>
        [HttpPost("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordInput input, CancellationToken cancellationToken)
        {
            return Ok(await _userInfoAppService.UpdatePassword(input));
        }

        /// <summary>
        /// 创建API签名
        /// </summary>
        /// <returns></returns>
        [HttpPost("CreateSignInfo")]
        public async Task<IActionResult> CreateSignInfo()
        {
            return Ok(await _signInfoAppService.CreateSignInfo(HttpContext));
        }

        /// <summary>
        /// 创建API签名
        /// </summary>
        /// <returns></returns>
        [HttpGet("CreateSignInfo")]
        public async Task<IActionResult> CreateSignInfoPost()
        {
            return Ok(await _signInfoAppService.CreateSignInfo(HttpContext));
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        [AllowAnonymous]
        [HttpGet(nameof(AESEncrypt))]
        public async Task<IActionResult> AESEncrypt(string str, string key, string iv) => Ok(await _userInfoAppService.AESEncrypt(str, key, iv));

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        [AllowAnonymous]
        [HttpGet(nameof(AESDecrypt))]
        public async Task<IActionResult> AESDecrypt(string str, string key, string iv) => Ok(await _userInfoAppService.AESDecrypt(str, key, iv));

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        [AllowAnonymous]
        [HttpGet(nameof(DESEncrypt))]
        public async Task<IActionResult> DESEncrypt(string str, string key, string iv) => Ok(await _userInfoAppService.DESEncrypt(str, key, iv));

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        [AllowAnonymous]
        [HttpGet(nameof(DESDecrypt))]
        public async Task<IActionResult> DESDecrypt(string str, string key, string iv) => Ok(await _userInfoAppService.DESDecrypt(str, key, iv));
    }
}