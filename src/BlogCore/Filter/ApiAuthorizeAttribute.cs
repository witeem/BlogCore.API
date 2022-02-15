using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace BlogCore.Filter
{
    public class ApiAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.Items["User"];
            bool signApi = Convert.ToBoolean(context.HttpContext.Items["Sign"]);
            if (signApi == false)
            {
                context.Result = new JsonResult(new { message = "API签名错误", Status = 403 })
                { StatusCode = StatusCodes.Status403Forbidden };
            }
            else {
                //验证是否需要授权和授权信息
                if (HasAllowAnonymous(context) == false && user == null)
                {
                    // not logged in
                    context.Result = new JsonResult(new { message = "Unauthorized", Status = 401 })
                    { StatusCode = StatusCodes.Status401Unauthorized };
                }
            }
        }

        private static bool HasAllowAnonymous(AuthorizationFilterContext context)
        {
            var filters = context.Filters;
            if (filters.OfType<IAllowAnonymousFilter>().Any())
            {
                return true;
            }

            // 在执行端点路由时，MVC不会为AllowOnyMousAttribute添加AllowOnyMousFilters
            // 在控制器和操作上发现。为了与2.x兼容
            // 我们将检查端点元数据中是否存在IALOWonymous。
            var endpoint = context.HttpContext.GetEndpoint();
            return endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null;
        }
    }
}
