// 创建人：魏天华 
// 测试添加代码文件头

using AutoMapper;
using BlogCore.Application.BloggerHandler.Model;
using BlogCore.Application.UserInfo;
using BlogCore.Application.UserInfo.Dtos;
using BlogCore.Domain.Comm.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlogCore.Application.BloggerHandler
{
    public class ArticleHandler : BaseHandler, IRequestHandler<RequestModel<BloggerArticleDto>, BloggerArticleDto>
    {
        private readonly IMapper _mapper;
        private readonly IBloggerAppservice _bloggerAppservice;


        public ArticleHandler(IMapper mapper, IBloggerAppservice bloggerAppservice)
        {
            _mapper = mapper;
            _bloggerAppservice = bloggerAppservice;
        }

        public async Task<BloggerArticleDto> Handle(RequestModel<BloggerArticleDto> request, CancellationToken cancellationToken)
        {
            switch (request.Type)
            {
                case Enums.RequestTypeEnum.GetBloggerInfo:
                    return (await _bloggerAppservice.GetBloggerArticleAsync(request.Id)).Data;
                case Enums.RequestTypeEnum.EditBloggerInfo:
                    return null;
                default:
                    return null;
            }
        }
    }
}
