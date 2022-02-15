// 创建人：魏天华 
// 测试添加代码文件头

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Core.CommonHelper.CommonDto
{
    public class ApiPageRequest
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页行数
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// 总数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public List<string> SortFields { get; set; }

        public ApiPageRequest()
        {
            PageIndex = 1;
            PageSize = 10;
            SortFields = new List<string> { "Id DESC" };
            Total = 0;
        }
    }
}
