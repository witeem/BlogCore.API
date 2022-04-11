using AutoMapper;
using BlogCore.Application.RoleInfo.Dtos;
using BlogCore.Application.UserInfo.Dtos;
using BlogCore.Core.RoleInfo;
using BlogCore.Core.UserInfo;

namespace BlogCore.Application.Maps
{
    public class AutoMapProfile : Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public AutoMapProfile()
        {
            CreateMap<AdverUserInfo, AdverUserInfoDto>().ReverseMap();
            CreateMap<AdverRoleInfo, AdverRoleInfoDto>().ReverseMap();
            CreateMap<BloggerInfo, BloggerInfoDto>().ReverseMap();
            CreateMap<BloggerArticle, BloggerArticleDto>().ReverseMap();
            //以下使用AutoMapAttribute映射，详见Dto
            //CreateMap<AdverUserInfo, AdverUserInfoDto>()
            //    .ForMember(dto => dto.RoleCodes,
            //    info => info.MapFrom(i => i.RoleCodes != null ? i.RoleCodes.ToString() : string.Empty));
        }
    }
}
