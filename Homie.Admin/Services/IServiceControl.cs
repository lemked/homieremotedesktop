using System;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace Homie.Admin.Services
{
    public interface IServiceControl
    {
        Task<ServiceControllerStatus> GetStatusAsync();

        Task StartServiceAsync(TimeSpan timeout);

        Task StopServiceAsync(TimeSpan timeout);

        Task PauseServiceAsync(TimeSpan timeout);

        Task ContinueServiceAsync(TimeSpan timeout);

        event EventHandler<ServiceControllerStatus> StatusChanged;
    }
}
