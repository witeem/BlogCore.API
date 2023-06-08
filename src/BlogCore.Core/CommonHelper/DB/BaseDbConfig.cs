// 创建人：魏天华 
// 测试添加代码文件头

using BlogCore.Core.CommonHelper.CommonDto;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using witeem.CoreHelper.ExtensionTools;
using witeem.CoreHelper.ExtensionTools.CommonTools;
using witeem.CoreHelper.ExtensionTools.Dtos;

namespace BlogCore.Core.CommonHelper.DB
{
    public static class BaseDbConfig
    {
        public static (DataBaseOperate MasterDb, List<DataBaseOperate> SlaveDbs, List<DataBaseOperate> OtherDbs) GetDataBaseOperate =>
            InitDataBaseConn();

        private static (DataBaseOperate, List<DataBaseOperate>, List<DataBaseOperate>) InitDataBaseConn()
        {
            DataBaseOperate masterDb = null;
            var slaveDbs = new List<DataBaseOperate>();
            var allDbs = new List<DataBaseOperate>();
            List<DataBaseOperate> otherDbs = new();
            List<SqlSugarDBS> sugarDBs = ConfigManagerHelper.GetSection<List<SqlSugarDBS>>("SqlSugarDBS");
            var appSetting = ConfigManagerHelper.GetSection<AppSetting>(BlogCoreConsts.AppSetting);
            if (sugarDBs.Any())
            {
                foreach (var item in sugarDBs)
                {
                    if (item.Enabled)
                    {
                        allDbs.Add(new DataBaseOperate
                        {
                            ConnId =item.ConnId,
                            HitRate = item.HitRate,
                            ConnectionString = item.ConnectionString,
                            IsSlave = item.IsSlave,
                            DbType = (DbType)item.DBType
                        });
                    }
                }
            }

            if (allDbs.Count < 1)
            {
                throw new System.Exception("请确保appsettings.json中配置连接字符串,并设置Enabled为true;");
            }

            string mainDB = ConfigManagerHelper.GetValue("MainDB");
            masterDb = allDbs.FirstOrDefault(x => x.ConnId == mainDB);
            if (masterDb == null)
            {
                throw new System.Exception($"请确保主库ID:{mainDB}的Enabled为true;");
            }

            //如果开启读写分离
            if (ConfigManagerHelper.GetValue("CQRSEnabled") == "true")
            {
                slaveDbs = allDbs.Where(x => x.DbType == masterDb.DbType && x.ConnId != mainDB)
                    .ToList();
                if (slaveDbs.Count < 1)
                {
                    throw new System.Exception($"请确保主库ID:{mainDB}对应的从库的Enabled为true;");
                }
            }

            otherDbs = allDbs.Where(x => x.ConnId != mainDB && !x.IsSlave).ToList();
            return (masterDb, slaveDbs, otherDbs);
        }
    }
}
