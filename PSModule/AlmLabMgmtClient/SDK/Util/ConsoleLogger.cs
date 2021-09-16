using PSModule.AlmLabMgmtClient.SDK.Interface;
using System;
using System.Management.Automation;
using System.Runtime.CompilerServices;

namespace PSModule.AlmLabMgmtClient.SDK.Util
{
    using C = Constants;
    public class ConsoleLogger : ILogger
    {
        private bool _printLineFeed = true;
        private readonly object _sync = new object();
        public void LogInfo(string msg)
        {
            lock(_sync)
            {
                if (_printLineFeed)
                {
                    Console.Write(C.LINE_FEED);
                    _printLineFeed = false;
                }
                Console.WriteLine(msg);
            }
        }

        public void LogError(string err, ErrorCategory categ = ErrorCategory.NotSpecified, bool isCritical = false, [CallerMemberName] string methodName = "")
        {
            lock (_sync)
            {
                if (_printLineFeed)
                {
                    Console.Write(C.LINE_FEED);
                    _printLineFeed = false;
                }
                Console.WriteLine(err);
            }
        }

        public void ShowProgress(char @char)
        {
            lock (_sync)
            {
                Console.Write(@char);
                if (!_printLineFeed)
                    _printLineFeed = true;
            }
        }
    }
}
