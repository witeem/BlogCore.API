// 创建人：魏天华 
// 测试添加代码文件头

using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Core.CommonHelper.DB
{
    public class DataBaseOperate
    {
        public string ConnId { get; set; }
        public int HitRate { get; set; }
        public string ConnectionString { get; set; }
        public DbType DbType { get; set; }
    }
}
