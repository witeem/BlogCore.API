using BlogCore.Core;
using BlogCore.Core.UserInfo;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using witeem.CoreHelper.ExtensionTools.CommonTools;
using witeem.CoreHelper.Redis;

namespace BlogCore.Domain.DomainServices.Advertisement
{
    public class AdverUserInfoDomainServices : IAdverUserInfoDomainServices
    {
        private readonly ILogger<AdverUserInfoDomainServices> _logger;
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly JwtSettings _jwtSettings;
        private readonly IRedisManager _redisManager;

        public AdverUserInfoDomainServices(
            ILogger<AdverUserInfoDomainServices> logger, 
            IUserInfoRepository userInfoRepository,
            IOptionsMonitor<JwtSettings> jwtSettings,
            IRedisManager redisManager)
        {
            _logger = logger;
            _userInfoRepository = userInfoRepository;
            _jwtSettings = jwtSettings.CurrentValue;
            _redisManager = redisManager;
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="input"></param>
        public async Task<AdverUserInfo> AddUserInfo(AdverUserInfo input)
        {
            if (input != null)
            {
                var result = await _userInfoRepository.AddReturnBoolAsync(input);
                if (result)
                {
                    return await _userInfoRepository.QueryFirstAsync(m => m.Name == input.Name && m.Phone == input.Phone);
                }
            }
            return null;
        }

        /// <summary>
        /// 部分数据需要解密展示
        /// </summary>
        /// <param name="userInfo"></param>
        public void SetDesDecrypt(AdverUserInfo userInfo)
        {
            if (userInfo != null)
            {
                userInfo.Password = EncyptHelper.DESDecrypt(userInfo.Password);
                userInfo.Phone = EncyptHelper.DESDecrypt(userInfo.Phone);
            }
        }

        /// <summary>
        /// 创建Token 令牌
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public async Task<string> GetJwtToken(AdverUserInfo userInfo)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var authTime = DateTime.UtcNow;
            var expiresAt = authTime.AddHours(2);
            IDictionary<string, object> claims = new Dictionary<string, object>();
            var tokenDescriptor = SetTokenDescriptor(userInfo, authTime, expiresAt, claims); // token 描述
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            await _redisManager.SetAsync<AdverUserInfo>(userInfo.Phone + userInfo.Password, userInfo, TimeSpan.FromDays(1)); // 用户登录状态存储
            return tokenString;
        }

        /// <summary>
        /// token 描述
        /// </summary>
        /// <param name="userInfo">用户信息实体</param>
        /// <param name="authTime">有效时间 起始</param>
        /// <param name="expiresAt">有效时间 截止</param>
        /// <param name="claims">token 保存内容</param>
        /// <returns></returns>
        private SecurityTokenDescriptor SetTokenDescriptor(AdverUserInfo userInfo, DateTime authTime, DateTime expiresAt, IDictionary<string, object> claims)
        {
            claims.Add("name", userInfo.Name);
            claims.Add("password", userInfo.Password);
            claims.Add("phone", userInfo.Phone);
            claims.Add("roleCodes", userInfo.RoleCodes);
            claims.Add("nickName", userInfo.NickName);
            claims.Add("birthDay", userInfo.BirthDay);
            claims.Add("sex", userInfo.Sex);
            claims.Add("age", userInfo.Age);
            var key = Encoding.ASCII.GetBytes(_jwtSettings.RawSigningKey);
            SecurityKey securityKey = new SymmetricSecurityKey(key);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = _jwtSettings.Issuer,   //签名者
                Audience = _jwtSettings.Audience,      //签名标识
                Claims = claims,
                Expires = expiresAt,
                IssuedAt = authTime,
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            return tokenDescriptor;
        }
    }
}
