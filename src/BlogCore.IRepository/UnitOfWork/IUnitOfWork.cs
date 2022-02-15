using BlogCore.Core.ModulesInterface;
using SqlSugar;
public interface IUnitOfWork : IRepositoryCore
{
    SqlSugarClient DbClient();

    void BeginTran();

    void CommitTran();

    void RollbackTran();
}
