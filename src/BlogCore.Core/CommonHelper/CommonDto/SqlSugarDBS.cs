// 创建人：魏天华 
// 测试添加代码文件头

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Core.CommonHelper.CommonDto
{
    public class SqlSugarDBS
    {
        public string ConnId { get; set; }

        public int HitRate { get; set; }

        public int DBType { get; set; }

        public bool Enabled { get; set; }

        public string ConnectionString { get; set; }
    }
}
