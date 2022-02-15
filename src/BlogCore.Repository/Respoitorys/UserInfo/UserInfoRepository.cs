// 创建人：魏天华 
// 测试添加代码文件头

using BlogCore.Core.UserInfo;

public class UserInfoRepository : SugarHelperClient<AdverUserInfo>, IUserInfoRepository
{
    public UserInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {

    }
}
