// 创建人：魏天华 
// 测试添加代码文件头

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Core.CommonHelper.CommonDto
{
    public static partial class EntityInitObject
    {
        /// <summary>
        /// 初始实体信息
        /// </summary>
        /// <param name="entity"></param>
        public static void InitEntity(this object entity)
        {
            var currentUser = NullDySession.Instance.CurrentUser;
            if (entity.ContainsProperty("CreateTime"))
                entity.SetPropertyValue("CreateTime", DateTime.Now);
            if (entity.ContainsProperty("Creator"))
                entity.SetPropertyValue("Creator", currentUser.Name);
            if (entity.ContainsProperty("LastModiftTime"))
                entity.SetPropertyValue("LastModiftTime", DateTime.Now);
            if (entity.ContainsProperty("LastModiftUser"))
                entity.SetPropertyValue("LastModiftUser", currentUser.Name);
            if (entity.ContainsProperty("IsDel"))
                entity.SetPropertyValue("IsDel", false);
        }

        /// <summary>
        /// 编辑实体信息
        /// </summary>
        /// <param name="entity"></param>
        public static void EditEntity(this object entity)
        {
            var currentUser = NullDySession.Instance.CurrentUser;
            if (entity.ContainsProperty("LastModiftTime"))
                entity.SetPropertyValue("LastModiftTime", DateTime.Now);
            if (entity.ContainsProperty("LastModiftUser"))
                entity.SetPropertyValue("LastModiftUser", currentUser.Name);
        }

        /// <summary>
        /// 编辑实体信息
        /// </summary>
        /// <param name="entity"></param>
        public static void DelEntity(this object entity)
        {
            var currentUser = NullDySession.Instance.CurrentUser;
            if (entity.ContainsProperty("LastModiftTime"))
                entity.SetPropertyValue("LastModiftTime", DateTime.Now);
            if (entity.ContainsProperty("LastModiftUser"))
                entity.SetPropertyValue("LastModiftUser", currentUser.Name);
            if (entity.ContainsProperty("IsDel"))
                entity.SetPropertyValue("IsDel", true);
        }

        /// <summary>
        /// 是否拥有某属性
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public static bool ContainsProperty(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName, BindingFlags.Public) != null;
        }

        /// <summary>
        /// 设置某属性值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static void SetPropertyValue(this object obj, string propertyName, object value)
        {
            obj.GetType().GetProperty(propertyName, BindingFlags.Public).SetValue(obj, value);
        }
    }
}
