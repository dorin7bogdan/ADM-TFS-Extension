using PSModule.AlmLabMgmtClient.SDK.Interface;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PSModule.AlmLabMgmtClient.SDK.Request
{
    public abstract class RequestBase : IRequest
    {
        protected readonly IClient _client;
        protected readonly ILogger _logger;
        protected const string X_XSRF_TOKEN = "X-XSRF-TOKEN";
        protected const string PROC_RUNS = "procedure-runs";

        protected RequestBase(IClient client)
        {
            _client = client;
            _logger = _client.Logger;
        }

        public async Task<Response> Execute()
        {
            Response res;
            try
            {
                res = await Perform();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                res = new Response(ex.Message);
            }

            return res;
        }

        public abstract Task<Response> Perform();

        protected virtual string Suffix => null;

        protected virtual WebHeaderCollection Headers => new WebHeaderCollection { { X_XSRF_TOKEN, _client.XsrfTokenValue } };

        protected string Body => null;

        protected virtual string Url => _client.BuildRestEndpoint(Suffix);
    }
}