// 创建人：魏天华 
// 测试添加代码文件头

using BlogCore.Application.UserInfo.Dtos;
using BlogCore.Application.UserInfo.Reqs;
using BlogCore.Core.UserInfo;
using BlogCore.Domain.Comm.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogCore.Application.UserInfo
{
    public interface IBloggerAppservice : IBaseServices<BloggerInfo>, IAppService
    {
        Task<ApiResponce<BloggerInfoDto>> GetBloggerInfoAsync();

        Task<ApiResponce<BloggerArticleDto>> GetBloggerArticleAsync(long id);

        Task<ApiResponce<List<BloggerArticleDto>>> GetBloggerArticlesAsync();

        Task<ApiResponce<bool>> AddViews(ArticleAddViewsReq req);
    }
}
