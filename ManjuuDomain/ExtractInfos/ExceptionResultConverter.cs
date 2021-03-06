using System;
using ManjuuDomain.HealthCheck;
using ManjuuDomain.IDomain;
using ManjuuDomain.Suppers;

namespace ManjuuDomain.ExtractInfos
{
    public class ExceptionResultConverter : SupResultConverter, IResultConverter
    {
        public ExceptionResultConverter(string ipV4, string port, string remarks, DateTime receiveTime)
         : base(ipV4, port, remarks, receiveTime)
        {
        }

        public override CheckReesultInfo Convert(ExtractInfo extractInfo)
        {
             return new CheckReesultInfo(IpV4,Port,Remarks,extractInfo.InfoType,ReceiveTime);
        }
    }
}