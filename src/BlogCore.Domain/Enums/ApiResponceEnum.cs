// 创建人：魏天华 
// 测试添加代码文件头

using System;
using System.Collections.Generic;
using System.Text;

namespace BlogCore.Domain.Enums
{
    public enum ApiResponceEnum
    {
        UnAuthorization = 1401,
        Undefind = 1404,
        Error = 1502,
        ParamsError = 1504,
        SignError = 1999
    }
}
