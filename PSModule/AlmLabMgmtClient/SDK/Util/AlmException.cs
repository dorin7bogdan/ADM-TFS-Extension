using System;
using System.Runtime.Serialization;

namespace PSModule.AlmLabMgmtClient.SDK.Util
{
    [Serializable]
    public class AlmException : Exception
    {
        public AlmException(string message) : base(message) { }
        protected AlmException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            // https://rules.sonarsource.com/csharp/RSPEC-3925
        }

    }
}
