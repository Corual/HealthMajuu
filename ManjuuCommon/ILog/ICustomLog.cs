using System;
using System.Collections.Generic;
using System.Text;

namespace ManjuuCommon.ILog
{
    public interface ICustomLog<T>
        where T : class
    {
        T GetLogger();
    }
}

