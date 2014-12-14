using System.ServiceModel;
using System.Threading.Tasks;

namespace Homie.Common.Interfaces
{
    [ServiceContract]
    public interface IWebServiceSession
    {
        [OperationContract]
        Task ConnectAsync();

        [OperationContract]
        bool Authenticate(string user, string pPassword);
    }
}
