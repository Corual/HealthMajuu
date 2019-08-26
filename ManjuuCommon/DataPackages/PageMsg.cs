using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManjuuCommon.DataPackages
{
    public class PageMsg<T> : BaseMsg
    {
        /// <summary>
        /// 页数据
        /// </summary>
        public List<T> PageData { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage { get; set; }

        /// <summary>
        /// 每页数据数
        /// </summary>
        public int EachPageDataCount { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 前面显示页
        /// </summary>
        public int PrevPage { get; set; }

        /// <summary>
        /// 后面显示页
        /// </summary>
        public int AfterPage { get; set; }

        public PageMsg()
        {
                
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageCapacity">页容量，默认20</param>
        public PageMsg(List<T> list, int currentPage, int pageCapacity=20)
        {
            PageData = list;
            EachPageDataCount = pageCapacity;
            CurrentPage = currentPage;
             GetPageInfo();
        }

        private PageMsg<T> GetPageInfo()
        {
            if (null == PageData || ! PageData.Any())
            {
                return this;
            }


            //计算当前页码
            CurrentPage = CurrentPage < 1 ? 1 : CurrentPage;

            //计算页容量
            EachPageDataCount = EachPageDataCount < 1 ? 20 : EachPageDataCount;

            //计算最大页数
            TotalPage = (int)Math.Ceiling(TotalPage * 1.0 / EachPageDataCount);

            //最大页区间
            int maxPageSpan = 2;

            //计算当前页之前的开始页码
            if (CurrentPage == TotalPage)
            {
                PrevPage = CurrentPage - (maxPageSpan * 2);
            }
            else
            {
                PrevPage = CurrentPage - maxPageSpan;
            }

            //防止变负数
            PrevPage = PrevPage > 1 ? PrevPage : 1;

            //计算当前页之后的结束页码
            AfterPage = PrevPage + (maxPageSpan * 2);

            //防止超过最大页数
            AfterPage = AfterPage > TotalPage ? TotalPage : AfterPage;

            return this;
        }
    }
}
