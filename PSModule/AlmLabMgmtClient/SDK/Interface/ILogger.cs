
using System.Management.Automation;
using System.Runtime.CompilerServices;

namespace PSModule.AlmLabMgmtClient.SDK.Interface
{
    public interface ILogger
    {
        public void LogInfo(string msg);
        public void ShowProgress(char @char);
        public void LogError(string err, ErrorCategory categ = ErrorCategory.NotSpecified, bool isCritical = false, [CallerMemberName] string methodName = "");

    }
}
