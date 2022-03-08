// 创建人：魏天华 
// 测试添加代码文件头

using BlogCore.Application.AdverUserHandler.Enums;
using BlogCore.Application.UserInfo.Dtos;
using MediatR;

namespace BlogCore.Application.AdverUserHandler.Model
{
    public class AdverUserInfoModel : IRequest<AdverUserInfoDto>
    {
        public ModelTypeEnum Type { get; set; }

        public AdverUserInfoDto AddParams { get; set; }

        public UserSearchDto SearchParams { get; set; }
    }
}
