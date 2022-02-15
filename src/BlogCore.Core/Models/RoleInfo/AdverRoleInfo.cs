using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogCore.Core.RoleInfo
{
    [SugarTable("t_adverroleinfo")]
    public class AdverRoleInfo : EntityBase
    {
        /// <summary>
        /// 角色编码
        /// </summary>
        public string RoleCode { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 角色等级
        /// </summary>
        public int RoleLevel { get; set; }
    }
}
