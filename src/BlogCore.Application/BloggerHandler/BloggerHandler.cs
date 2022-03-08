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
using witeem.CoreHelper.ExtensionTools.CommonTools;

namespace BlogCore.Application.BloggerHandler
{
    public class BloggerHandler : BaseHandler, IRequestHandler<BloggerInfoModel, BloggerInfoDto>
    {
        private readonly IMapper _mapper;
        private readonly IBloggerAppservice _bloggerAppservice;


        public BloggerHandler(IMapper mapper, IBloggerAppservice bloggerAppservice)
        {
            _mapper = mapper;
            _bloggerAppservice = bloggerAppservice;
        }

        public async Task<BloggerInfoDto> Handle(BloggerInfoModel request, CancellationToken cancellationToken)
        {
            switch (request.Type)
            {
                case Enums.BloggerTypeEnum.GetBloggerInfo:
                    return (await _bloggerAppservice.GetBloggerInfoAsync()).Data;
                case Enums.BloggerTypeEnum.EditBloggerInfo:
                    return null;
                default:
                    return null;
            }
        }
    }
}
