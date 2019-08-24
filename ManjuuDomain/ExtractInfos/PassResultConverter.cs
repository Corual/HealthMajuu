using System;
using ManjuuDomain.HealthCheck;
using ManjuuDomain.IDomain;
using ManjuuDomain.Suppers;

namespace ManjuuDomain.ExtractInfos
{
    public class PassResultConverter : SupResultConverter,IResultConverter
    {
        public PassResultConverter(string ipV4, string port, string remarks, DateTime receiveTime) 
        : base(ipV4, port, remarks, receiveTime)
        {

        }

        public override CheckReesultInfo Convert(ExtractInfo extractInfo)
        {
            //todo:读取预设超时时间
            int presettime = 1000;
           return new CheckReesultInfo(IpV4,Port,Remarks,extractInfo.InfoType,extractInfo.Maxtime,extractInfo.MinTime,extractInfo.AvgTime, presettime, ReceiveTime);
        }
    }
}