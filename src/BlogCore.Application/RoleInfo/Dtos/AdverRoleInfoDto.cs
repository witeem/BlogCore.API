using BlogCore.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogCore.Application.RoleInfo.Dtos
{
    public class AdverRoleInfoDto : EntityBase
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
