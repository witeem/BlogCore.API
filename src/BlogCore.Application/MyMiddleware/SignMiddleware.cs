// 创建人：魏天华 
// 测试添加代码文件头

using BlogCore.Core;
using BlogCore.Domain.Comm.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using witeem.CoreHelper.ExtensionTools;
using witeem.CoreHelper.ExtensionTools.CommonTools;
using witeem.CoreHelper.HttpHelper;

namespace BlogCore.Application.MyMiddleware
{
    public class SignMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpClient _httpClient;
        private readonly AppSetting _appSetting;

        public SignMiddleware(RequestDelegate next, IHttpClient httpClient, IOptionsMonitor<AppSetting> appSetting)
        {
            _next = next;
            _httpClient = httpClient;
            _appSetting = appSetting.CurrentValue;
        }

        public async Task Invoke(HttpContext context)
        {
            List<string> ignoreApiList = ConfigManagerHelper.GetSection<List<string>>("IgnoreApiSign");
            string method = context.Request.Path;
            string timeSpan = context.Request.Query["timespan"];
            string nonce = context.Request.Query["nonce"];
            string sign = context.Request.Query["sign"];
            if (ignoreApiList != null)
            {
                foreach (var item in ignoreApiList)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        if (method.ToLower().Contains(item.ToLower()))
                        {
                            context.Items["Sign"] = true;
                            await _next(context);
                            return;
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(timeSpan) || string.IsNullOrEmpty(nonce) || string.IsNullOrEmpty(sign))
            {
                //签名验证失败
                context.Items["Sign"] = false;
                await _next(context);
                return;
            }

            try
            {
                if (!timeSpan.IsNumber())
                {
                    //签名验证失败
                    context.Items["Sign"] = false;
                    await _next(context);
                    return;
                }

                DateTimeOffset offTime = new DateTimeOffset(DateTime.Now);
                long tSpan = offTime.ToUnixTimeMilliseconds() - timeSpan.ToInt64();
                if (tSpan > 3600 * 1000 * 10) // 时间擢大于10分钟返回出错
                {
                    //签名验证失败
                    context.Items["Sign"] = false;
                    await _next(context);
                    return;
                }

                await Identify(context, timeSpan, nonce, sign);
            }
            catch (Exception ex)
            {
                //签名验证失败
                context.Items["Sign"] = false;
                Console.WriteLine(ex.StackTrace);
            }

            await _next(context);
        }

        /// <summary>
        /// 签名认证
        /// </summary>
        /// <param name="context"></param>
        /// <param name="timeSpan"></param>
        /// <param name="nonce"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        private async Task Identify(HttpContext context, string timeSpan, string nonce, string sign)
        {
            var secretKey = await GetSecretKey(context);
            string signIdentify = EncyptHelper.MD5(secretKey + timeSpan + nonce);
            if (sign == signIdentify)
            {
                context.Items["Sign"] = true;
            }
            else
            {
                //签名验证失败
                context.Items["Sign"] = false;
            }
        }

        /// <summary>
        /// 请求参数加密返回
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        private async Task<string> GetSecretKey(HttpContext httpContext)
        {
            if (httpContext.Request.Method == "GET")
            {
                IQueryCollection keyValues = httpContext.Request.Query;
                Dictionary<string, string> dic = new Dictionary<string, string>();
                List<string> authorityKeys = new List<string>() { "timespan", "nonce", "sign" };
                foreach (var item in keyValues)
                {
                    if (!authorityKeys.Contains(item.Key.ToLower()))
                    {
                        dic.Add(item.Key, item.Value);
                    }
                }

                return EncyptHelper.AESEncrypt(JsonConvert.SerializeObject(dic), _appSetting.ConnKey, _appSetting.ConnIV);
            }
            else
            {
                httpContext.Request.EnableBuffering();
                var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8);
                httpContext.Request.Body.Seek(0, SeekOrigin.Begin);  //大概是== Request.Body.Position = 0;的意思
                var readerStr = await reader.ReadToEndAsync();
                httpContext.Request.Body.Seek(0, SeekOrigin.Begin);  //读完后也复原

                object obj = JsonConvert.DeserializeObject(readerStr);
                return EncyptHelper.AESEncrypt(JsonConvert.SerializeObject(obj), _appSetting.ConnKey, _appSetting.ConnIV);
            }
        }
    }
}
