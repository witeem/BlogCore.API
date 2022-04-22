// 创建人：魏天华 
// 测试添加代码文件头

using BlogCore.Core.Menus;
using BlogCore.Core.RoleInfo;
using BlogCore.Core.UserInfo;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Core.Models
{
    public static class DataBaseOperate
    {
        public static void CreateTable()
        {
            try
            {
                SqlSugarClient sqlSugarClient = new SqlSugarClient(new ConnectionConfig
                {
                    DbType = DbType.MySql,
                    ConnectionString = "Server=127.0.0.1;port=3306;database=localhostDB;uid=user;password=localhost;Convert Zero Datetime=True;Allow User Variables=True;pooling=true",
                    IsAutoCloseConnection = true
                });

                #region 创建数据库和表的语句仅执行一次
                sqlSugarClient.DbMaintenance.CreateDatabase();
                sqlSugarClient.CodeFirst.InitTables(typeof(AdverUserInfo));
                sqlSugarClient.CodeFirst.InitTables(typeof(BloggerInfo));
                sqlSugarClient.CodeFirst.InitTables(typeof(AdverRoleInfo));
                sqlSugarClient.CodeFirst.InitTables(typeof(RoleMenus));
                sqlSugarClient.CodeFirst.InitTables(typeof(AdverMenus));
                sqlSugarClient.CodeFirst.InitTables(typeof(BloggerArticle));
                #endregion

                sqlSugarClient.Aop.OnLogExecuted = (sql, pra) =>
                {
                    Console.WriteLine("******************************************************************");
                    Console.WriteLine($"Sql语句：{sql}");
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
