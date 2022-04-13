// 创建人：魏天华 
// 测试添加代码文件头

using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Core.UserInfo
{
    [SugarTable("t_blogger_article")]
    public class BloggerArticle : EntityBase
    {
        public long BloggerId { get; set; }

        public string ArticleTitle { get; set; }

        public string Introduction { get; set; }

        public string Article { get; set; }

        public string Label { get; set; }

        public int Views { get; set; }

        public int Likes { get; set; }
    }
}
