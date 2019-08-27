using System;
using ManjuuCommon.Tools;
using ManjuuInfrastructure.Repository.Enum;

namespace ManjuuInfrastructure.Repository.Entity {
    /// <summary>
    /// 基础的实体
    /// </summary>
    public abstract class BaseEntity {
        /// <summary>
        /// 数据库标识
        /// </summary>
        /// <value></value>
        public int Id { get; set; }

        /// <summary>
        /// 数据创建时间
        /// </summary>
        /// <value></value>
        public DateTime CreateTime { get; set; } = TimeMgr.GetLoaclDateTime();

        /// <summary>
        /// 数据最后更新时间
        /// </summary>
        /// <value></value>
        public DateTime LastUpdateTime { get; set; } = TimeMgr.GetLoaclDateTime();

        /// <summary>
        /// 数据状态
        /// </summary>
        /// <value></value>
        public DataState State { get; set; } = DataState.Enable;

    }
}