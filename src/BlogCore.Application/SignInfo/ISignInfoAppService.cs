// 创建人：魏天华 
// 测试添加代码文件头

using BlogCore.Domain.Comm.Dto;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BlogCore.Application.SignInfo
{
    public  interface ISignInfoAppService : IAppService
    {
        /// <summary>
        /// 根据参数生成签名
        /// </summary>
        /// <param name="input">input</param>
        Task<dynamic> CreateSignInfo(HttpContext httpContext);
    }
}
