using BlogCore.Core.UserInfo;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using witeem.CoreHelper.ExtensionTools.CommonTools;

namespace BlogCore.Core
{
    /// <summary>
    /// 当前会话对象
    /// </summary>
    public class NullDySession
    {
        /// <summary>
        /// 获取DySession实例
        /// </summary>
        public static NullDySession Instance { get; } = new NullDySession();
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        public AdverUserInfo CurrentUser
        {
            get
            {
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

        private NullDySession()
        {
        }
    }
}
