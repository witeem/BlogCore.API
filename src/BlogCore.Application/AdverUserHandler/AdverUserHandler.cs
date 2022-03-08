// 创建人：魏天华 
// 测试添加代码文件头

using AutoMapper;
using BlogCore.Application.AdverUserHandler.Enums;
using BlogCore.Application.AdverUserHandler.Model;
using BlogCore.Application.UserInfo;
using BlogCore.Application.UserInfo.Dtos;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using witeem.CoreHelper.ExtensionTools.CommonTools;

namespace BlogCore.Application.AdverUserHandler
{
    public class AdverUserHandler : BaseHandler, IRequestHandler<AdverUserInfoModel, AdverUserInfoDto> 
    {
        private readonly IMapper _mapper;
        private readonly IUserInfoAppService _userInfoAppService;
        public AdverUserHandler(IMapper mapper, IUserInfoAppService userInfoAppService)
        {
            _mapper = mapper;
            _userInfoAppService = userInfoAppService;
        }

        public async Task<AdverUserInfoDto> Handle(AdverUserInfoModel request, CancellationToken cancellationToken)
        {
            switch (request.Type)
            {
                case ModelTypeEnum.Add:
                    return (await _userInfoAppService.AddUserInfoAsync(request.AddParams)).Data;
                case ModelTypeEnum.Update:
                    return null;
                case ModelTypeEnum.Del:
                    return null;
                case ModelTypeEnum.Query:
                    return (await _userInfoAppService.GetUserInfoAsync(request.SearchParams)).Data;
                default:
                    return null;
            }
            
        }
    }
}
