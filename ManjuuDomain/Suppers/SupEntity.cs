using System;
using System.Collections.Generic;
using System.Text;

namespace ManjuuDomain.Suppers
{
    /// <summary>
    /// 实体
    /// </summary>
    public abstract class SupEntity
    {
        /// <summary>
        /// 实体的id（非业务性质的唯一标识）
        /// </summary>
        public int Id { get; protected set; }
    }
}
