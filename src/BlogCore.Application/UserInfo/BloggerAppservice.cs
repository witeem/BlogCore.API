// 创建人：魏天华 
// 测试添加代码文件头

using AutoMapper;
using BlogCore.Application.UserInfo.Dtos;
using BlogCore.Application.UserInfo.Reqs;
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
using witeem.CoreHelper.Redis;

namespace BlogCore.Application.UserInfo
{
    public class BloggerAppservice : BaseServices<BloggerInfo>, IBloggerAppservice
    {
        private readonly ILogger<BloggerAppservice> _logger;
        private readonly IBloggerInfoRepository _bloggerInfoRepository;
        private readonly IBloggerArticleRepository _bloggerArticleRepository;
        private readonly IMapper _mapper;
        private readonly IRedisManager _redisManager;

        public BloggerAppservice(ILogger<BloggerAppservice> logger, IBloggerInfoRepository bloggerInfoRepository, IBloggerArticleRepository bloggerArticleRepository, IMapper mapper,
            IRedisManager redisManager)
        {
            _logger = logger;
            _bloggerInfoRepository = bloggerInfoRepository;
            _bloggerArticleRepository = bloggerArticleRepository;
            _mapper = mapper;
            _redisManager = redisManager;
        }

        /// <summary>
        /// 获取博主信息
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
                    .Select(m => new BloggerArticleDto
                    {
                        Id = m.Id,
                        ArticleTitle = m.ArticleTitle,
                        BloggerId = m.BloggerId,
                        Introduction = m.Introduction,
                        Label = m.Label,
                        Views = m.Views,
                        Likes = m.Likes
                    }).ToListAsync();
                return ApiResponce<List<BloggerArticleDto>>.Success(articles);
            }
            catch
            {

                throw;
            }
        }

        public async Task<ApiResponce<bool>> AddViews(ArticleAddViewsReq req)
        {
            try
            {
                if (await _redisManager.ExistsAsync($"{req.Id}-{req.IP}"))
                {
                    return ApiResponce<bool>.Success(true);
                }

                var article = await _bloggerArticleRepository.QueryableAsync(m => m.Id == req.Id).Select(m => new BloggerArticle
                {
                    Id = m.Id,
                    Label = m.Label,
                    LastModiftTime = m.LastModiftTime,
                    LastModiftUser = m.LastModiftUser,
                    Likes = m.Likes,
                    Views = m.Views
                }).FirstAsync();
                if (article != null)
                {
                    article.Views += 1;
                    _ = await _bloggerArticleRepository.UpdateAsync(article, m => m.Id == req.Id, c => c.Views);
                    await _redisManager.SetAsync($"{req.Id}-{req.IP}", 1, TimeSpan.FromHours(1));
                }

                return ApiResponce<bool>.Success(true);
            }
            catch
            {
                throw;
            }
        }
    }
}
