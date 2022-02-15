using BlogCore.Core.Serivces;
using BlogCore.Core.UserInfo;
using BlogCore.Domain.DomainServices.Entitys;
using System.Threading.Tasks;

namespace BlogCore.Domain.DomainServices.Advertisement
{
    public interface IAdverUserInfoDomainServices: IDomainService
    {
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<AdverUserInfo> AddUserInfo(AdverUserInfo input);

        /// <summary>
        /// 部分数据需要解密展示
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        Task SetDesDecrypt(AdverUserInfo userInfo);

        /// <summary>
        /// 获取登录用户token
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        Task<string> GetJwtToken(AdverUserInfo userInfo);
    }
}
