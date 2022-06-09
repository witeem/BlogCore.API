using AutoMapper;
using BlogCore.Application.UserInfo.Dtos;
using BlogCore.Core;
using BlogCore.Core.Menus;
using BlogCore.Core.RoleInfo;
using BlogCore.Core.UserInfo;
using BlogCore.Domain.Comm.Dto;
using BlogCore.Domain.DomainServices.Advertisement;
using BlogCore.Domain.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SqlSugar;
using System;
using System.Linq;
using System.Threading.Tasks;
using witeem.CoreHelper.ExtensionTools.CommonTools;
using witeem.CoreHelper.Redis;

namespace BlogCore.Application.UserInfo
{
    public class UserInfoAppService: BaseServices<AdverUserInfo> , IUserInfoAppService
    {
        private readonly ILogger<UserInfoAppService> _logger;
        private readonly JwtSettings _jwtSettings;
        private readonly AppSetting _appSetting;
        private readonly IMapper _mapper;
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IAdverUserInfoDomainServices _userInfoDomainServices;
        private readonly IRedisManager _redisManager;
        private readonly IRoleMenusRepository _roleMenusRepository;
        private readonly IMenusInfoRepository _menusInfoRepository;

        public UserInfoAppService(ILogger<UserInfoAppService> logger, IOptionsMonitor<JwtSettings> jwtSettings, IOptionsMonitor<AppSetting> appSetting,
            IMapper mapper, IUserInfoRepository userInfoRepository, IAdverUserInfoDomainServices adverUserInfoDomainServices,
            IRedisManager redisManager, IRoleMenusRepository roleMenusRepository, IMenusInfoRepository menusInfoRepository)
        {
            _logger = logger;
            _jwtSettings = jwtSettings.CurrentValue;
            _appSetting = appSetting.CurrentValue;
            _mapper = mapper;
            _userInfoRepository = userInfoRepository;
            _userInfoDomainServices = adverUserInfoDomainServices;
            _redisManager = redisManager;
            _roleMenusRepository = roleMenusRepository;
            _menusInfoRepository = menusInfoRepository;
        }

        #region OAuthController

        /// <summary>
        /// 注销当前用户
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponce<bool>> LogOut()
        {
            // 当前会话对象，当currentUser不为null。 则表示Token校验通过
            var currentUser = NullDySession.Instance.CurrentUser;
            if (currentUser != null)
            {
                // 删除当前用户存储在Redis的缓存信息
                await _redisManager.DelAsync(currentUser.Phone + currentUser.Password);
            }

            return ApiResponce<bool>.Success(true);
        }

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponce<AdverUserInfo>> GetAuthenticate()
        {
            await Task.CompletedTask;
            var currentUser = NullDySession.Instance.CurrentUser;
            if (currentUser != null)
            {
                var userInfo = await _redisManager.GetAsync<AdverUserInfo>(currentUser.Phone + currentUser.Password);
                return ApiResponce<AdverUserInfo>.Success(userInfo);
            }

            return ApiResponce<AdverUserInfo>.Fail((int)ApiResponceEnum.UnAuthorization, "授权失败");
        }

        /// <summary>
        /// 获取菜单信息
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponce<dynamic>> GetMenus()
        {
            var getUserInfo = await GetAuthenticate();
            if (getUserInfo.Code == 200)
            {
                var userInfo = getUserInfo.Data;
                var roles = userInfo.RoleCodes.Split(",").ToList();
                var myMenus = await _menusInfoRepository.QueryMuchAsync<AdverMenus, RoleMenus, AdverMenus>((x, y) => new object[] { JoinType.Inner, x.MenuCode == y.MenuCode },
                    (x, y) => x, (x, y) => roles.Contains(y.RoleCode) && x.IsDel == false);
                return ApiResponce<dynamic>.Success(myMenus);
            }

            return ApiResponce<dynamic>.Fail((int)ApiResponceEnum.UnAuthorization, "授权失败");
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ApiResponce<bool>> UpdatePassword(UpdatePasswordInput input)
        {
            var userInfo = (await GetAuthenticate())?.Data;
            if (userInfo != null)
            {
                if (input.OldPassword != userInfo.Password)
                {
                    return ApiResponce<bool>.Fail((int)ApiResponceEnum.Error, "原密码错误");
                }

                var userEntity = _mapper.Map<AdverUserInfo>(userInfo);
                userEntity.Password = EncyptHelper.DESEncrypt(input.NewPassword);
                var result = await _userInfoRepository.UpdateColumnsAsync(userEntity, m => m.Password, w => w.Id);
                return ApiResponce<bool>.Success(result > 0);
            }
            return ApiResponce<bool>.Fail((int)ApiResponceEnum.UnAuthorization, "授权失败");
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ApiResponce<AdverUserInfoDto>> GetUserInfoAsync(UserSearchDto input)
        {
            try
            {
                input.Password = EncyptHelper.DESEncrypt(input.Password);
                var userInfo = await _userInfoRepository.QueryFirstAsync(m => m.Name == input.UserName && m.Password == input.Password);
                _userInfoDomainServices.SetDesDecrypt(userInfo);
                return ApiResponce<AdverUserInfoDto>.Success(_mapper.Map<AdverUserInfoDto>(userInfo));
            }
            catch
            {

                throw;
            }
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ApiResponce<AdverUserInfoDto>> AddUserInfoAsync(AdverUserInfoDto input)
        {
            try
            {
                var existUser = await _userInfoRepository.QueryFirstAsync(m => m.Name == input.Name && !m.IsDel);
                if (existUser != null)
                {
                    return ApiResponce<AdverUserInfoDto>.Fail((int)ApiResponceEnum.Error, "用户名已存在");
                }

                var currentUser = NullDySession.Instance.CurrentUser;
                var _input = _mapper.Map<AdverUserInfo>(input);
                _input.Password = EncyptHelper.DESEncrypt(input.Password);
                _input.Phone = EncyptHelper.DESEncrypt(input.Phone);
                _input.CreateTime = DateTime.Now;
                _input.Creator = currentUser?.Name ?? input.Name;
                _input.LastModiftTime = DateTime.Now;
                _input.LastModiftUser = currentUser?.Name ?? input.Name;
                var userInfo = await _userInfoDomainServices.AddUserInfo(_input);
                return ApiResponce<AdverUserInfoDto>.Success(_mapper.Map<AdverUserInfoDto>(userInfo));
            }
            catch
            {

                throw;
            }
        }

        public async Task<ApiResponce<string>> GetJwtToken(AdverUserInfoDto input)
        {
            try
            {
                var userInfo = _mapper.Map<AdverUserInfo>(input);
                string token = await _userInfoDomainServices.GetJwtToken(userInfo);
                return ApiResponce<string>.Success(token);
            }
            catch
            {

                throw;
            }
        }
        #endregion

        #region EncyptHelper
        public async Task<ApiResponce<string>> AESEncrypt(string str, string key, string iv)
        {
            string connStr = EncyptHelper.AESEncrypt(str, key, iv);
            return await Task.FromResult(ApiResponce<string>.Success(connStr));
        }

        public async Task<ApiResponce<string>> AESDecrypt(string str, string key, string iv)
        {
            string connStr = EncyptHelper.AESDecrypt(str, key, iv);
            return await Task.FromResult(ApiResponce<string>.Success(connStr));
        }

        public async Task<ApiResponce<string>> DESEncrypt(string str, string key, string iv)
        {
            string connStr = EncyptHelper.DESEncrypt(str, key, iv);
            return await Task.FromResult(ApiResponce<string>.Success(connStr));
        }

        public async Task<ApiResponce<string>> DESDecrypt(string str, string key, string iv)
        {
            string connStr = EncyptHelper.DESDecrypt(str, key, iv);
            return await Task.FromResult(ApiResponce<string>.Success(connStr));
        }
        #endregion
    }
}
