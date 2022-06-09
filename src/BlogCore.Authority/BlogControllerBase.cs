using BlogCore.Core.UserInfo;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using witeem.CoreHelper.ExtensionTools.CommonTools;

namespace BlogCore.Authority
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BlogControllerBase : ControllerBase
    {
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        public AdverUserInfo CurrentUser {
            get {
                var claimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;

                var claimsIdentity = claimsPrincipal?.Identity as ClaimsIdentity;

                var userClaim = claimsIdentity?.Claims.FirstOrDefault(c => c.Type == "adverUser");
                if (userClaim == null || string.IsNullOrEmpty(userClaim.Value))
                {
                    return null;
                }

                return userClaim.Value.ToObject<AdverUserInfo>();
            }
        }
    }
}
