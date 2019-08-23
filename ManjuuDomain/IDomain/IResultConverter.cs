using System;
using ManjuuDomain.ExtractInfos;
using ManjuuDomain.HealthCheck;

namespace ManjuuDomain.IDomain
{
    public interface IResultConverter
    {
         CheckReesultInfo Convert(ExtractInfo extractInfo);
    }
}