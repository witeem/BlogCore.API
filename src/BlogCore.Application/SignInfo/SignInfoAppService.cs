// 创建人：魏天华 
// 测试添加代码文件头

using BlogCore.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using witeem.CoreHelper.ExtensionTools.CommonTools;
using witeem.CoreHelper.Redis;

namespace BlogCore.Application.SignInfo
{
    public class SignInfoAppService : ISignInfoAppService
    {
        private readonly ILogger<SignInfoAppService> _logger;
        private readonly IRedisManager _redisManager;
        private readonly AppSetting _appSetting;

        public SignInfoAppService(ILogger<SignInfoAppService> logger, IRedisManager redisManager, IOptionsMonitor<AppSetting> appSetting)
        {
            _logger = logger;
            _redisManager = redisManager;
            _appSetting = appSetting.CurrentValue;
        }

        /// <summary>
        /// 创建签名认证
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task<dynamic> CreateSignInfo(HttpContext httpContext)
        {
            DateTimeOffset offTime = new DateTimeOffset(DateTime.Now);
            string timeSpan = offTime.ToUnixTimeMilliseconds().ToString();
            string nonce = Guid.NewGuid().ToString();
            var secretKey = await GetSecretKey(httpContext);
            string sign = EncyptHelper.MD5(secretKey + timeSpan + nonce);
            return new { TimeSpan = timeSpan, Nonce = nonce, Sign = sign };
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
