using System;
using System.Collections.Generic;
using System.Text;

namespace ManjuuInfrastructure.Repository.Entity
{
    public class MachineInfo: BaseEntity
    {
        /// <summary>
        /// ipv4地址
        /// </summary>
        public string IpAddressV4 { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public string Port { get; set; } = "80";

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

    }
}
