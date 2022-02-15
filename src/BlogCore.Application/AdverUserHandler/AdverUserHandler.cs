// 创建人：魏天华 
// 测试添加代码文件头

using AutoMapper;
using BlogCore.Application.AdverUserHandler.Enums;
using BlogCore.Application.AdverUserHandler.Model;
using BlogCore.Application.UserInfo;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using witeem.CoreHelper.ExtensionTools.CommonTools;

namespace BlogCore.Application.AdverUserHandler
{
    public class AdverUserHandler : BaseHandler, IRequestHandler<AdverUserInfoModel, string> 
    {
        private readonly IMapper _mapper;
        private readonly IUserInfoAppService _userInfoAppService;
        public AdverUserHandler(IMapper mapper, IUserInfoAppService userInfoAppService)
        {
            _mapper = mapper;
            _userInfoAppService = userInfoAppService;
        }

        public async Task<string> Handle(AdverUserInfoModel request, CancellationToken cancellationToken)
        {
            switch (request.Type)
            {
                case ModelTypeEnum.Add:
                    return (await _userInfoAppService.AddUserInfoAsync(request.AddParams)).ToJson();
                case ModelTypeEnum.Update:
                    return "";
                case ModelTypeEnum.Del:
                    return "";
                case ModelTypeEnum.Query:
                    return (await _userInfoAppService.GetUserInfoAsync(request.SearchParams)).ToJson();
                default:
                    return "";
            }
            
        }
    }
}
