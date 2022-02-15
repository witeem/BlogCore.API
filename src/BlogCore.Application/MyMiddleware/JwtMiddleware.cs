using BlogCore.Application.UserInfo.Dtos;
using BlogCore.Core;
using BlogCore.Core.UserInfo;
using BlogCore.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using witeem.CoreHelper.ExtensionTools;
using witeem.CoreHelper.ExtensionTools.CommonTools;
using witeem.CoreHelper.Redis;

namespace BlogCore.Application.MyMiddleware
{
    /// <summary>
    /// JWT校验中间件
    /// </summary>
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtSettings _jwtSettings;
        private readonly IRedisManager _redisManager;

        public JwtMiddleware(RequestDelegate next, IOptionsMonitor<JwtSettings> jwtSettings, IRedisManager redisManager)
        {
            _next = next;
            _jwtSettings = jwtSettings.CurrentValue;
            _redisManager = redisManager;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //获取上传token，可自定义扩展
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()
                        ?? context.Request.Headers["X-Token"].FirstOrDefault()
                        ?? context.Request.Query["Token"].FirstOrDefault()
                        ?? context.Request.Cookies["Token"];

            if (token != null)
                AttachUserToContext(context, token);

            await _next(context);
        }

        private void AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                var key = Encoding.ASCII.GetBytes(_jwtSettings.RawSigningKey);
                SecurityKey securityKey = new SymmetricSecurityKey(key);
                var validateParameter = new TokenValidationParameters()
                {
                    ValidateLifetime = _jwtSettings.ValidateLifetime,
                    ValidateAudience = _jwtSettings.ValidateAudience,
                    ValidateIssuer = _jwtSettings.ValidateIssuer,
                    ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
                    ValidIssuer = _jwtSettings.Issuer,   //签名者
                    ValidAudience = _jwtSettings.Audience,      //签名标识
                    IssuerSigningKey = securityKey,
                };

                //校验并解析token
                var claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(token, validateParameter, out SecurityToken validatedToken);//validatedToken:解密后的对象
                var jwtPayload = ((JwtSecurityToken)validatedToken).Payload.SerializeToJson(); //获取payload中的数据 
                AdverUserInfo adverUser = jwtPayload.ToObject<AdverUserInfo>();
                var currentUser = _redisManager.Get<AdverUserInfoDto>(adverUser.Phone + adverUser.Password);
                if (currentUser == null)
                {
                    context.Items["User"] = null;
                    return;
                }

                //写入认证信息，方便业务类使用
                var claimsIdentity = new ClaimsIdentity(new Claim[] { new Claim(OAuthConsts.OAuthUser, jwtPayload) });
                Thread.CurrentPrincipal = new ClaimsPrincipal(claimsIdentity);

                // attach user to context on successful jwt validation
                context.Items["User"] = adverUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                context.Items["User"] = null;
            }
        }
    }
}
