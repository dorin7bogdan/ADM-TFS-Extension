using PSModule.AlmLabMgmtClient.SDK.Interface;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PSModule.AlmLabMgmtClient.SDK.Util
{
    public class ConsoleLogger : ILogger
    {
        private readonly static char[] DOTs = new string('.', 100).ToCharArray();
        private int _count = 0;
        public async Task LogInfo(string msg)
        {
            await Console.Out.WriteLineAsync(msg);
            if (_count > 0)
                _count = 0;
        }

        public async Task LogError(string err, [CallerMemberName] string methodName = "")
        {
            await Console.Error.WriteLineAsync($"{methodName}: {err}");
            if (_count > 0)
                _count = 0;
        }

        public async Task ShowProgress()
        {
            if (_count == DOTs.Length)
                _count = 0;

            await Console.Out.WriteLineAsync(DOTs, 0, ++_count);
        }
    }
}
