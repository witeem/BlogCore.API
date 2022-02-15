// 创建人：魏天华 
// 测试添加代码文件头

using System;
using System.Collections.Generic;
using System.Text;

namespace BlogCore.Application.SignInfo.Dtos
{
    public class SignAuthorityDto
    {
        public string TimeSpan { get; set; }

        public string Nonce { get; set; }

        public string SecretKey { get; set; }
    }
}
