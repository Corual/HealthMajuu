using System;
using System.Collections.Generic;
using System.Text;
using ManjuuDomain.Suppers;
namespace ManjuuDomain.HealthCheck
{
    public class CheckTarget : SupEntity
    {
        /// <summary>
        /// ip地址
        /// </summary>
        public string IpAddres { get; private set; }

        /// <summary>
        /// 目标端口号
        /// </summary>
        public string TargetPort { get; private set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; private set; }

    }
}
