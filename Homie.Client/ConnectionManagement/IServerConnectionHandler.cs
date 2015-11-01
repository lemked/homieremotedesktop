using System.Threading.Tasks;

namespace Homie.Client.ConnectionManagement
{
    interface IServerConnectionHandler : IConnectionHandler
    {
        Task ConnectAsync();
    }
}
