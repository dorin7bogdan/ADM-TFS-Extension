using System.Threading.Tasks;

namespace PSModule.AlmLabMgmtClient.SDK.Interface
{
    public interface IAuthenticator
    {
        Task<bool> Login(IClient client, string username, string password, string clientType);
        Task<bool> Logout(IClient client);

    }
}
