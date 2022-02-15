// 创建人：魏天华 
// 测试添加代码文件头

using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogCore.Core.Menus
{
    [SugarTable("t_advermenus")]
    public class AdverMenus : EntityBase
    {
        public string MenuName { get; set; }

        public int Level { get; set; }

        public string MenuCode { get; set; }

        public string Path { get; set; }

        public string ParentCode { get; set; }

        public bool IsMenu { get; set; }
    }
}
