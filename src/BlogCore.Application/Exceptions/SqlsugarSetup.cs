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
                    masterDb = new ConnectionConfig
                    {
                        ConfigId = BaseDbConfig.GetDataBaseOperate.MasterDb.ConnId.ToLower(),
                        ConnectionString = BaseDbConfig.GetDataBaseOperate.MasterDb.ConnectionString,
                        DbType = BaseDbConfig.GetDataBaseOperate.MasterDb.DbType,
                        IsAutoCloseConnection = true,
                        AopEvents = new AopEvents
                        {
                            OnLogExecuting = (sql, pars) => //执行前
                            {
                                Parallel.For(0, 1, _ =>
                                {
                                    if (ConfigManagerHelper.GetValue("IsSqlAOP") == "true")
                                    {
                                        LogHelper.WriteSqlLog($"SqlLog{DateTime.Now:yyyy-MM-dd}",
                                            new[] { "【SQL参数】：\n" + GetParams(pars), "【SQL语句】" + sql });
                                    }
                                });
                            }
                            //OnLogExecuted = //执行完毕
                        },
                        MoreSettings = new ConnMoreSettings
                        {
                            IsAutoRemoveDataCache = true
                        },
                        // 从库
                        SlaveConnectionConfigs = slaveDbs
                    };
                }

                return new SqlSugarClient(masterDb);
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
    }
}
