// 创建人：魏天华 
// 测试添加代码文件头

using BlogCore.Core.Menus;

public class MenusInfoRepository : SugarHelperClient<AdverMenus>, IMenusInfoRepository
{
    public MenusInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {

    }
}
