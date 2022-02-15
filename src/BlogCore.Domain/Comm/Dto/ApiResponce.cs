// 创建人：魏天华 
// 测试添加代码文件头

using System;
using System.Collections.Generic;
using System.Text;

namespace BlogCore.Domain.Comm.Dto
{
    public class ApiResponce<T>
    {
        public int Code { get; set; }

        public T Data { get; set; }

        public string Msg { get; set; }

        public static ApiResponce<T> Success(T datas, string msg = "操作成功")
        {
            ApiResponce<T> responce = new ApiResponce<T>()
            {
                Code = 200,
                Data = datas,
                Msg = msg
            };
            return responce;
        }

        public static ApiResponce<T> Fail(int code, string msg)
        {
            ApiResponce<T> responce = new ApiResponce<T>()
            {
                Code = code,
                Data = default(T),
                Msg = msg
            };
            return responce;
        }
    }
}
