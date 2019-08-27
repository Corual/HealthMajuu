using System;
using System.Collections.Generic;
using System.Text;

namespace ManjuuDomain.Dto
{
    public class EquipmentDto
    {

        public int Id { get; set; }

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
