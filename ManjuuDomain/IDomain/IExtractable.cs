using System;
using System.Collections.Generic;
using System.Text;
using ManjuuDomain.ExtractInfos;


namespace ManjuuDomain.IDomain
{
    /// <summary>
    /// 提取抽象
    /// </summary>
    public interface IExtractable
    {
        ExtractInfo Extract(string result);
    }

    

    
}
