using PSModule.AlmLabMgmtClient.SDK.Handler;
using PSModule.AlmLabMgmtClient.SDK.Interface;
using PSModule.AlmLabMgmtClient.SDK.Util;

namespace PSModule.AlmLabMgmtClient.SDK.Factory
{
    using C = Constants;
    public class PollHandlerFactory
    {
        public PollHandler Create(IClient client, string runType, string entityId)
        {
            PollHandler ret;
            if (runType.In(C.BVS, C.TEST_SET))
            {
                ret = new LabPollHandler(client, entityId);
            }
            else
            {
                throw new AlmException("PollHandlerFactory: Unrecognized run type");
            }

            return ret;
        }

        public PollHandler Create(IClient client, string runType, string entityId, int interval)
        {
            PollHandler ret;
            if (runType.In(C.BVS, C.TEST_SET))
            {
                ret = new LabPollHandler(client, entityId, interval);
            }
            else
            {
                throw new AlmException("PollHandlerFactory: Unrecognized run type");
            }

            return ret;
        }
    }
}