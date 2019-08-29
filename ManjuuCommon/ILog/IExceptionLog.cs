using System;
using System.Collections.Generic;
using System.Text;

namespace ManjuuCommon.ILog
{
    public interface IExceptionLog<T> : ICustomLog<T>
        where T : class
    {
    }
}
