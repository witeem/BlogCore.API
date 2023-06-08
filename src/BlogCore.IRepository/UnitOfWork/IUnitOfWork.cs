using BlogCore.Core.ModulesInterface;
using SqlSugar;
public interface IUnitOfWork : IRepositoryCore
{
    SqlSugarScope DbClient();

    void BeginTran();

    void CommitTran();

    void RollbackTran();
}
