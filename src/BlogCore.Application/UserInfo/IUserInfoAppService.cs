using BlogCore.Application.UserInfo.Dtos;
using BlogCore.Core.UserInfo;
using BlogCore.Domain.Comm.Dto;
using System.Threading.Tasks;

namespace BlogCore.Application.UserInfo
{
    public interface IUserInfoAppService : IBaseServices<AdverUserInfo>, IAppService
    {
        /// <summary>
        /// 注销当前用户
        /// </summary>
        /// <returns></returns>
        Task<ApiResponce<bool>> LogOut();

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        Task<ApiResponce<AdverUserInfo>> GetAuthenticate();

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="input">input</param>
        Task<ApiResponce<AdverUserInfoDto>> GetUserInfoAsync(UserSearchDto input);

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="input">input</param>
        Task<ApiResponce<AdverUserInfoDto>> AddUserInfoAsync(AdverUserInfoDto input);

        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ApiResponce<string>> GetJwtToken(AdverUserInfoDto input);

        /// <summary>
        /// 获取菜单信息
        /// </summary>
        /// <returns></returns>
        Task<ApiResponce<dynamic>> GetMenus();

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ApiResponce<bool>> UpdatePassword(UpdatePasswordInput input);

        Task<ApiResponce<string>> AESEncrypt(string str, string key, string iv);

        Task<ApiResponce<string>> AESDecrypt(string str, string key, string iv);

        Task<ApiResponce<string>> DESEncrypt(string str, string key, string iv);

        Task<ApiResponce<string>> DESDecrypt(string str, string key, string iv);
    }
}
