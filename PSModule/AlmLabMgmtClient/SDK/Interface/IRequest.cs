using System.Threading.Tasks;

namespace PSModule.AlmLabMgmtClient.SDK.Interface
{
    public interface IRequest
    {
        Task<Response> Execute();
    }
}
