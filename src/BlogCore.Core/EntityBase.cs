using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogCore.Core
{
    public abstract class EntityBase
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public virtual long  Id { get; set; }

        public virtual DateTime CreateTime { get; set; }

        public virtual string Creator { get; set; }

        public virtual DateTime LastModiftTime { get; set; }

        public virtual string LastModiftUser { get; set; }

        public virtual bool IsDel { get; set; }
    }
}
