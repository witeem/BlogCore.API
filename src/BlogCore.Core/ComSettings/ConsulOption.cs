// 创建人：魏天华 
// 测试添加代码文件头

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Core.ComSettings
{
    public class ConsulOption
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 服务IP
        /// </summary>
        public string ServiceIP { get; set; }

        /// <summary>
        /// 服务端口
        /// </summary>
        public int ServicePort { get; set; }

        /// <summary>
        /// 服务健康检查地址
        /// </summary>
        public string ServiceHealthCheck { get; set; }

        /// <summary>
        /// Consul 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 权重
        /// </summary>
        public string Weight { get; set; }
    }
}
