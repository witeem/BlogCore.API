// 创建人：魏天华 
// 测试添加代码文件头

using AutoMapper;
using BlogCore.Application.UserInfo.Dtos;
using BlogCore.Core.UserInfo;
using BlogCore.Domain.Comm.Dto;
using BlogCore.IRepository.Respoitorys.UserInfo;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using witeem.CoreHelper.ExtensionTools.CommonTools;

namespace BlogCore.Application.UserInfo
{
    public class BloggerAppservice : BaseServices<BloggerInfo>, IBloggerAppservice
    {
        private readonly ILogger<BloggerAppservice> _logger;
        private readonly IBloggerInfoRepository _bloggerInfoRepository;
        private readonly IBloggerArticleRepository _bloggerArticleRepository;
        private readonly IMapper _mapper;

        public BloggerAppservice(ILogger<BloggerAppservice> logger, IBloggerInfoRepository bloggerInfoRepository, IBloggerArticleRepository bloggerArticleRepository, IMapper mapper)
        {
            _logger = logger;
            _bloggerInfoRepository = bloggerInfoRepository;
            _bloggerArticleRepository = bloggerArticleRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ApiResponce<BloggerInfoDto>> GetBloggerInfoAsync()
        {
            try
            {
                var userInfo = await _bloggerInfoRepository.QueryFirstAsync(m => m.UserId == 5);
                return ApiResponce<BloggerInfoDto>.Success(_mapper.Map<BloggerInfoDto>(userInfo));
            }
            catch
            {

                throw;
            }
        }

        public async Task<ApiResponce<BloggerArticleDto>> GetBloggerArticleAsync(long id)
        {
            try
            {
                var article = await _bloggerArticleRepository.QueryFirstAsync(m => m.Id == id);
                return ApiResponce<BloggerArticleDto>.Success(_mapper.Map<BloggerArticleDto>(article));
            }
            catch
            {

                throw;
            }
        }   

        
        public async Task<ApiResponce<List<BloggerArticleDto>>> GetBloggerArticlesAsync()
        {
            try
            {
                var articles = await _bloggerArticleRepository.QueryableAsync(m => !m.IsDel && m.BloggerId == 1)
                    .Select(m => new BloggerArticleDto { Id = m.Id, ArticleTitle = m.ArticleTitle, BloggerId = m.BloggerId, Introduction = m.Introduction }).ToListAsync();
                return ApiResponce<List<BloggerArticleDto>>.Success(articles);
            }
            catch
            {

                throw;
            }
        }
    }
}
