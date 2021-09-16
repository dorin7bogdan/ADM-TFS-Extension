using PSModule.AlmLabMgmtClient.SDK.Interface;
using PSModule.AlmLabMgmtClient.SDK.Util;
using System.Threading;
using System.Threading.Tasks;

namespace PSModule.AlmLabMgmtClient.SDK.Handler
{
    using C = Constants;
    public abstract class PollHandler : HandlerBase
    {
        private readonly int _interval = 5000; // millisecond
        protected PollHandler(IClient client, string entityId) : base(client, entityId)
        {
        }

        protected PollHandler(IClient client, string entityId, int interval) : base(client, entityId)
        {
            _interval = interval;
        }

        protected PollHandler(IClient client, string entityId, string runId) : base(client, entityId, runId)
        {
        }

        public async Task<bool> Poll()
        {
            _logger.LogInfo($"Start Polling... Run ID: {_runId}");
            return await DoPoll();
        }

        protected virtual async Task<bool> DoPoll()
        {
            bool ok = false;
            int failures = 0;
            while (failures < 3)
            {
                var res = await GetResponse();
                if (res.IsOK)
                {
                    await LogProgress();
                    if (IsFinished(res))
                    {
                        ok = true;
                        LogRunEntityResults(await GetRunEntityResultsResponse());
                        break;
                    }
                    else
                    {
                        _logger.ShowProgress(C.DOT);
                    }
                }
                else
                {
                    LogPollingError(res);
                    ++failures;
                }
                Sleep();
            }

            return ok;
        }

        protected abstract Task<Response> GetRunEntityResultsResponse();

        protected abstract bool LogRunEntityResults(Response response);

        protected abstract bool IsFinished(Response response);

        protected abstract Task<Response> GetResponse();

        protected void Sleep()
        {
            try
            {
                Thread.Sleep(_interval);
            }
            catch (ThreadInterruptedException)
            {
                _logger.LogError("Interrupted while polling");
                throw;
            }
        }

        protected void LogPollingError(Response res)
        {
            _logger.LogError($"Polling try failed. Status code: {res.StatusCode}, Error: {res.Error ?? "Not Available"}");
        }

        protected abstract Task LogProgress();

    }
}