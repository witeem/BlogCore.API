// 创建人：魏天华 
// 测试添加代码文件头

using System;
using System.Collections.Generic;
using System.Text;

namespace BlogCore.Application.UserInfo.Dtos
{
    public class UpdatePasswordInput
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
