using System;
using System.Collections.Generic;
using System.Text;
using ManjuuDomain.Suppers;
namespace ManjuuDomain.HealthCheck
{
    public class CheckTarget : SupEntity
    {
        #region 属性
        /// <summary>
        /// ip地址
        /// </summary>
        public string IpAddresV4 { get; private set; }

        /// <summary>
        /// 目标端口号
        /// </summary>
        public string TargetPort { get; private set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; private set; }
        #endregion


        #region 构造函数
        public CheckTarget()
        {

        }

        public CheckTarget(string ipv4, string port, string remarks)
        {
            this.IpAddresV4 = ipv4;
            this.TargetPort = port;
            this.Remarks = remarks;
        }
        #endregion



    }
}
