using System;
using System.Collections.Generic;
using System.Text;

namespace ManjuuDomain.Dto
{
   public class DataBoxDto<T>
    {
        /// <summary>
        /// 当前箱子的数据
        /// </summary>
        public List<T> Data { get; set; }

        /// <summary>
        /// 可获取总数量
        /// </summary>
        public int Total { get; set; }
    }
}
