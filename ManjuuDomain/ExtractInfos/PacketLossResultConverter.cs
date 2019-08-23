using System;
using ManjuuDomain.HealthCheck;
using ManjuuDomain.IDomain;
using ManjuuDomain.Suppers;

namespace ManjuuDomain.ExtractInfos
{
    public class PacketLossResultConverter : SupResultConverter, IResultConverter
    {
        public PacketLossResultConverter(string ipV4, string port, string remarks, DateTime receiveTime)
         : base(ipV4, port, remarks, receiveTime)
        {
        }

        public override CheckReesultInfo Convert(ExtractInfo extractInfo)
        {
             //todo:读取预设超时时间
            int presettime = 0;
            return new CheckReesultInfo(IpV4,Port,Remarks,extractInfo.InfoType,extractInfo.LossRate,presettime,ReceiveTime);
        }
    }
}