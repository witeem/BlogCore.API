// 创建人：魏天华 
// 测试添加代码文件头

using BlogCore.Core.RoleInfo;

public class RoleInfoRepository : SugarHelperClient<AdverRoleInfo>, IRoleInfoRepository
{
    public RoleInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {

    }
}
