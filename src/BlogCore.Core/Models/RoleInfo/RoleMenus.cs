// 创建人：魏天华 
// 测试添加代码文件头

using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogCore.Core.RoleInfo
{
    [SugarTable("t_role_menus")]
    public class RoleMenus : EntityBase
    {
        public string MenuCode { get; set; }

        public string RoleCode { get; set; }
    }
}
