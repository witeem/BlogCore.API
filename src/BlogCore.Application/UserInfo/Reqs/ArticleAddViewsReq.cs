// 创建人：魏天华 
// 测试添加代码文件头

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Application.UserInfo.Reqs
{
    public class ArticleAddViewsReq
    {
        /// <summary>
        /// 文章ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 客户端IP地址
        /// </summary>
        public string IP { get; set; }
    }
}
