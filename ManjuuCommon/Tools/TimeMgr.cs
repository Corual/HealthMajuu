using System;
using System.Collections.Generic;
using System.Text;

namespace ManjuuCommon.Tools
{
   public   class TimeMgr
    {
        public static DateTime GetLoaclDateTime()
        {
            return TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Local);
        }
    }
}
