// 创建人：魏天华 
// 测试添加代码文件头

using BlogCore.Application.BloggerHandler.Enums;
using BlogCore.Domain.Comm.Dto;
using MediatR;

namespace BlogCore.Application.BloggerHandler.Model
{
    public class RequestModel<T> : IRequest<T>
    {
        public RequestTypeEnum Type { get; set; }

        public long Id { get; set; }
    }
}
