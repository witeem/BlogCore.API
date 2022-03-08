// 创建人：魏天华 
// 测试添加代码文件头
using BlogCore.Application.BloggerHandler.Enums;
using BlogCore.Application.UserInfo.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Application.BloggerHandler.Model
{
    public class BloggerInfoModel : IRequest<BloggerInfoDto>
    {
        public BloggerTypeEnum Type { get; set; }
    }
}
