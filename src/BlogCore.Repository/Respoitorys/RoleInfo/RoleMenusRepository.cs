// 创建人：魏天华 
// 测试添加代码文件头

using BlogCore.Core.RoleInfo;

public class RoleMenusRepository : SugarHelperClient<RoleMenus>, IRoleMenusRepository
{
    public RoleMenusRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {

    }
}
