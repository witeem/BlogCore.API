using Microsoft.Extensions.Logging;
using SqlSugar;

public class UnitOfWork : IUnitOfWork
{
    private readonly ISqlSugarClient _sqlSugarClient;
    private readonly ILogger<UnitOfWork> _logger;

    public UnitOfWork(ISqlSugarClient sqlSugarClient, ILogger<UnitOfWork> logger)
    {
        _sqlSugarClient = sqlSugarClient;
        _logger = logger;
    }

    /// <summary>
    /// 获取DB，保证唯一性
    /// </summary>
    /// <returns></returns>
    public SqlSugarScope DbClient()
    {
        return _sqlSugarClient as SqlSugarScope;
    }

    public void BeginTran()
    {
        DbClient().BeginTran();
    }

    public void CommitTran()
    {
        try
        {
            DbClient().CommitTran();
        }
        catch (Exception ex)
        {
            DbClient().RollbackTran();
            _logger.LogError($"{ex.Message}\r\n{ex.InnerException}");
            throw;
        }
    }

    public void RollbackTran()
    {
        DbClient().RollbackTran();
    }
}
