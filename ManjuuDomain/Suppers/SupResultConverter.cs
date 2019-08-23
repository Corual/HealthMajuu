using System;
using ManjuuDomain.ExtractInfos;
using ManjuuDomain.HealthCheck;
using ManjuuDomain.IDomain;

namespace ManjuuDomain.Suppers
{
    public abstract class SupResultConverter : IResultConverter
    {
        public string IpV4 { get; protected set; }
        public string Port { get; protected set; }
        public string Remarks { get; protected set; }
        public DateTime ReceiveTime { get; protected set; }
        public SupResultConverter(string ipV4, string port, string remarks, DateTime receiveTime)
        {
            IpV4 = ipV4;
            Port = port;
            Remarks = remarks;
            ReceiveTime = receiveTime;
        }
        public abstract CheckReesultInfo Convert(ExtractInfo extractInfo);
    }
}