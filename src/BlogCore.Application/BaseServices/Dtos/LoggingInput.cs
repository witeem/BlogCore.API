// 创建人：魏天华 
// 测试添加代码文件头

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Application.BaseServices.Dtos
{
    public class LoggingInput
    {
        public string Certificate { get; set; }

        public string Consumer { get; set; }

        public string Credential { get; set; }

        public string Kong_routing { get; set; }

        public string Message { get; set; }

        public LoggingRequest Request { get; set; }
    }

    public class LoggingRequest
    { 
        public string Headers { get; set; }

        public string Params { get; set; }

        public string Body { get; set; }
    }
}
