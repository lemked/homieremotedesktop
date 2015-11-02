using System;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace Homie.Admin.Services
{
    public class ServiceControl : IServiceControl
    {
        private readonly ServiceController service;

        public ServiceControl()
        {
            service = new ServiceController(Common.Constants.ServiceName);
        }

        public async Task<ServiceControllerStatus> GetStatusAsync()
        {
            return await Task.Factory.StartNew(() => GetStatus());
        }

        private ServiceControllerStatus GetStatus()
        {
            return service.Status;
        }

        public async Task StartServiceAsync(TimeSpan timeout)
        {
            await Task.Factory.StartNew(() => StartService(timeout));
        }

        private void StartService(TimeSpan timeout)
        {
            OnStatusChanged(ServiceControllerStatus.StartPending);
            service.Start();
            service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            OnStatusChanged(ServiceControllerStatus.Running);
        }

        public async Task StopServiceAsync(TimeSpan timeout)
        {
            await Task.Factory.StartNew(() => StopService(timeout));
        }

        private void StopService(TimeSpan timeout)
        {
            OnStatusChanged(ServiceControllerStatus.StopPending);
            service.Stop();
            service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
            OnStatusChanged(ServiceControllerStatus.Stopped);
        }

        public async Task PauseServiceAsync(TimeSpan timeout)
        {
            await Task.Factory.StartNew(() => PauseService(timeout));
        }

        public void PauseService(TimeSpan timeout)
        {
            OnStatusChanged(ServiceControllerStatus.PausePending);
            service.Pause();
            service.WaitForStatus(ServiceControllerStatus.Paused, timeout);
            OnStatusChanged(ServiceControllerStatus.Paused);
        }

        public async Task ContinueServiceAsync(TimeSpan timeout)
        {
            await Task.Factory.StartNew(() => ContinueService(timeout));
        }

        private void ContinueService(TimeSpan timeout)
        {
            OnStatusChanged(ServiceControllerStatus.ContinuePending);
            service.Continue();
            service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            OnStatusChanged(ServiceControllerStatus.Running);
        }

        public event EventHandler<ServiceControllerStatus> StatusChanged;

        protected virtual void OnStatusChanged(ServiceControllerStatus e)
        {
            StatusChanged?.Invoke(this, e);
        }
    }
}
