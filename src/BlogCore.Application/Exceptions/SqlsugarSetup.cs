// 创建人：魏天华 
// 测试添加代码文件头

using BlogCore.Core.CommonHelper.DB;
using BlogCore.Core.CommonHelper.Helper;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using witeem.CoreHelper.ExtensionTools;

namespace BlogCore.Application.Exceptions
{
    public static class SqlsugarSetup
    {
        public static void AddSqlSugarSetup(this IServiceCollection services)
        {
            services.AddScoped<ISqlSugarClient>(options =>
            {
                // 连接字符串
                ConnectionConfig masterDb = null; //主库
                // 从库
                var slaveDbs = new List<SlaveConnectionConfig>(); //从库列表
                List<ConnectionConfig> connConfigs = new List<ConnectionConfig>();
                BaseDbConfig.GetDataBaseOperate.SlaveDbs.ForEach(s =>
                {
                    slaveDbs.Add(new SlaveConnectionConfig()
                    {
                        HitRate = s.HitRate,
                        ConnectionString = s.ConnectionString
                    });
                });

                // 主库
                if (BaseDbConfig.GetDataBaseOperate.MasterDb != null)
                {
                    masterDb = CreateDbConfig(BaseDbConfig.GetDataBaseOperate.MasterDb);
                    masterDb.SlaveConnectionConfigs = slaveDbs;
                }

                connConfigs.Add(masterDb);

                // 其他数据库
                if (BaseDbConfig.GetDataBaseOperate.OtherDbs?.Count > 0)
                {
                    BaseDbConfig.GetDataBaseOperate.OtherDbs.ForEach(item => connConfigs.Add(CreateDbConfig(item)));
                }

                SqlSugarScope sqlSugar = new SqlSugarScope(connConfigs);
                return sqlSugar;
            });
        }

        /// <summary>
        /// 参数拼接字符串
        /// </summary>
        /// <param name="pars"></param>
        /// <returns></returns>
        private static string GetParams(SugarParameter[] pars)
        {
            return pars.Aggregate("", (current, p) => current + $"{p.ParameterName}:{p.Value}\n");
        }

        private static ConnectionConfig CreateDbConfig(DataBaseOperate item)
        {
            return new ConnectionConfig
            {
                ConfigId = item.ConnId,
                ConnectionString = item.ConnectionString,
                DbType = item.DbType,
                IsAutoCloseConnection = true,
                AopEvents = new AopEvents
                {
                    //执行前
                    OnLogExecuting = (sql, pars) =>
                    {
                        _ = Parallel.For(0, 1, _ =>
                        {
                            if (ConfigManagerHelper.GetSection<bool>("IsSqlAOP"))
                            {
                                LogHelper.WriteSqlLog($"SqlLog{DateTime.Now:yyyy-MM-dd}",
                                    new[] { $"【Sql Executeting】{DateTime.Now.ToString("G")} \n【SQL parameters inquiry】：\n" + GetParams(pars), "【T-SQL】" + sql });
                            }
                        });
                    },

                    //执行完毕
                    OnLogExecuted = (sql, pars) => {
                        _ = Parallel.For(0, 1, _ =>
                        {
                            if (ConfigManagerHelper.GetSection<bool>("IsSqlAOP"))
                            {
                                LogHelper.WriteSqlLog($"SqlLog{DateTime.Now:yyyy-MM-dd}",
                                    new[] { $"【Sql Executed】 {DateTime.Now.ToString("G")}" });
                            }
                        });
                    }
                },
                MoreSettings = new ConnMoreSettings
                {
                    IsAutoRemoveDataCache = true
                },
            };
        }
    }
}
