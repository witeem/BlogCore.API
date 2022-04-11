﻿// 创建人：魏天华 
// 测试添加代码文件头

using BlogCore.Core.UserInfo;
using BlogCore.IRepository.Respoitorys.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Repository.Respoitorys.UserInfo
{
    public class BloggerArticleRepository : SugarHelperClient<BloggerArticle>, IBloggerArticleRepository
    {
        public BloggerArticleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
