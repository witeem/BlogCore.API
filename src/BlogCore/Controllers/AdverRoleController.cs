using BlogCore.Application.RoleInfo;
using BlogCore.Core.RoleInfo;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlogCore.Controllers
{
    public class AdverRoleController : BlogControllerBase
    {
        private readonly IRoleInfoAppService _roleInfoAppService;

        public AdverRoleController(IRoleInfoAppService roleInfoAppService)
        {
            _roleInfoAppService = roleInfoAppService;
        }
    }
}
