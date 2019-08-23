using System.Collections.Generic;
using ManjuuDomain.ExtractInfos;
using ManjuuDomain.IDomain;
using ManjuuDomain.Suppers;

namespace ManjuuDomain.Chains
{
    /// <summary>
    /// window平台结果提取链
    /// </summary>
    public class WindowsExtractChain : IExtractable
    {
        private List<IExtractable> chainList = new List<IExtractable>();

        /// <summary>
        /// 添加提取器
        /// </summary>
        /// <param name="iextract"></param>
        /// <returns></returns>
        public WindowsExtractChain AddExtractor(IExtractable iextract)
        {
            chainList.Add(iextract);
            return this;
        }

        public ExtractInfo Extract(string result)
        {
            foreach (var item in chainList)
            {
               ExtractInfo info = item.Extract(result);
               if(ExtractInfo.ZeroInfo != info)
               {
                   return info;
               }
            }

            return ExtractInfo.ZeroInfo;
        }

    }
}