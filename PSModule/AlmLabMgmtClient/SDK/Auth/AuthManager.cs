using PSModule.AlmLabMgmtClient.SDK.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PSModule.AlmLabMgmtClient.SDK.Auth
{
    public sealed class AuthManager
    {
        private readonly IList<IAuthenticator> _authenticators;
        private static readonly AuthManager _instance = new AuthManager();

        private AuthManager()
        {
            _authenticators = new List<IAuthenticator>
            {
                new RestAuthenticator(),
                new ApiKeyAuthenticator()
            };
        }

        public static AuthManager Instance => _instance;

        public async Task<bool> Authenticate(IClient client, string username, string password, string clientType)
        {
            bool ok = false;
            foreach (IAuthenticator auth in _authenticators)
            {
                ok = await auth.Login(client, username, password, clientType);
                if (ok)
                    break;
            }
            return ok;
        }
        public async Task Logout(IClient client)
        {
            foreach (IAuthenticator auth in _authenticators)
                _ = await auth.Logout(client);
        }

    }
}
