using Niche.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Niche.Core.IServices
{
    public interface ILoggerService
    {
        void Log(LogLevel level, string message);
    }
}
