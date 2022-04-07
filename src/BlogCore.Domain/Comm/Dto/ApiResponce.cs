// 创建人：魏天华 
// 测试添加代码文件头

using BlogCore.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogCore.Domain.Comm.Dto
{
    public class ApiResponce<T>
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 返回泛型
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static ApiResponce<T> Success(T datas, string msg = "操作成功")
        {
            ApiResponce<T> responce = new ApiResponce<T>()
            {
                Code = (int)ApiResponceEnum.Succeed,
                Data = datas,
                Msg = msg
            };
            return responce;
        }

        /// <summary>
        /// 返回失败
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
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
