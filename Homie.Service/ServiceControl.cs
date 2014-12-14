using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceProcess;
using System.Threading.Tasks;

using Homie.Model.Logging;
using Homie.Common;
using Homie.Common.Interfaces;
using Homie.Common.Logging;
using MVVMLib;

using ServiceHost = System.ServiceModel.ServiceHost;

namespace Homie.Service
{
    public class ServiceControl : ServiceBase
    {
        private const string NoServiceArgument = "/NOSERVICE";
        private const string InstallServiceArgument = "/INSTALL";

        private CustomBinding CustomBinding
        {
            get
            {
                BindingElement lBindingElement = new TcpTransportBindingElement();
                CustomBinding lResult = new CustomBinding(lBindingElement);
                lResult.ReceiveTimeout = TimeSpan.MaxValue;
                lResult.CloseTimeout = TimeSpan.MaxValue;
                lResult.OpenTimeout = TimeSpan.MaxValue;
                lResult.SendTimeout = TimeSpan.MaxValue;
                return lResult;
            }
        }

        private ServiceHost serviceHost;

        public ServiceControl()
        {
            // Name the Windows Service
            ServiceName = Constants.ServiceName;

            // Configure database logger
            ILogger dbLogger = new DbLogger();
            dbLogger.LogLevel = LogLevel.Info;
            Log.Register(dbLogger);
        }

        /// <summary>
        /// The starting point of the service application.
        /// </summary>
        /// <param name="pArgs">The p arguments.</param>
        /// <author>Daniel Lemke - lemked@web.de</author>
        public static void Main(string[] pArgs)
        {
            // Register global event handlers for unhandled exceptions.
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerUnobservedTaskException;

            // Set up depedency injection.
            ServiceLocator.Register<IServiceLogDataSource, DbServiceLogDataSource>();
            ServiceLocator.Register<IServiceLogProvider, ServiceLogProvider>();
            ServiceLocator.Register<IMachineControlService, MachineControlService>();

            // Configure default logger
            ILogger textLogger = new FileLogger();
            textLogger.LogLevel = LogLevel.Trace;
            Log.Register(textLogger);
            
            foreach (string lArgument in pArgs)
            {
                if (lArgument.Equals(NoServiceArgument, StringComparison.InvariantCultureIgnoreCase))
                {
                    RunInConsoleMode();
                    return;
                }

                if (lArgument.Equals(InstallServiceArgument, StringComparison.InvariantCultureIgnoreCase))
                {
                    ManagedInstallerClass.InstallHelper(new[] { Assembly.GetExecutingAssembly().Location });
                    return;
                }
            }
            
            Run(new ServiceControl());
        }

        private static void TaskSchedulerUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Log.Exception(e.Exception);
        }

        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception lException = ExceptionUtils.UnwrapExceptionObject(e.ExceptionObject);
            Log.Exception(lException);
        }

        private static void RunInConsoleMode()
        {
            // Configure console logger
            ILogger consoleLogger = new ConsoleLogger();
            consoleLogger.LogLevel = LogLevel.Debug;
            Log.Register(consoleLogger);

            Log.Info("Service started with " + NoServiceArgument);
            var lServiceControl = new ServiceControl();
            lServiceControl.OnStart(new string[0]);

            Console.WriteLine(@"Service started, press any key to finish execution.");
            Console.ReadKey();
            lServiceControl.OnStop();
        }

        // Start the Windows service.
        protected override void OnStart(string[] pArgs)
        {
            Log.Info(ServiceName + " was started.");

            if (serviceHost != null)
            {
                // Close the service host if an instance already exists
                serviceHost.Close();
            }

            AddServiceEndpoint();
            AddMexEndpoint();

            if (serviceHost != null)
            {
                try
                {
                    serviceHost.Open();
                }
                catch (Exception lException)
                {
                    Log.Exception(lException);
                    throw;
                }
            }

            Log.Info(ServiceName + " is up and running.");
        }

        private void AddServiceEndpoint()
        {
            var lTcpBaseAddress = new Uri(String.Format(Constants.SERVICE_URL_TEMPLATE, "localhost", Properties.Settings.Default.ListenPort , Constants.SERVICE_URL_END_POINT));
            var lTcpServiceAddressTemplate = lTcpBaseAddress + "{0}/";
            try
            {
                serviceHost = new ServiceHost(typeof(HomieService), lTcpBaseAddress);
                
                Log.Debug("Adding service endpoints ...");
                var machineServiceEndPoint = String.Format(lTcpServiceAddressTemplate, Constants.MACHINECONTROLSERVICE_URL_END_POINT);
                serviceHost.AddServiceEndpoint(typeof(IMachineControlService), CustomBinding, machineServiceEndPoint);
                Log.Debug("Service endpoint added: " + machineServiceEndPoint);
                var serviceLogProviderEndPoint = String.Format(lTcpServiceAddressTemplate, Constants.SERVICELOGREADER_URL_END_POINT);
                serviceHost.AddServiceEndpoint(typeof(IServiceLogProvider), CustomBinding, serviceLogProviderEndPoint);
                Log.Debug("Service endpoint added: " + serviceLogProviderEndPoint);
            }
            catch (Exception lException)
            {
                Log.Exception(lException);
                throw;
            }
        }
        
        private void AddMexEndpoint()
        {
            BindingElement lBindingElement = new TcpTransportBindingElement();
            var lBinding = new CustomBinding(lBindingElement);

            try
            {
                Log.Debug("Adding MEX endpoint ...");
                ServiceMetadataBehavior lMetadataBehavior = serviceHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
                if (lMetadataBehavior == null)
                {
                    lMetadataBehavior = new ServiceMetadataBehavior();
                    serviceHost.Description.Behaviors.Add(lMetadataBehavior);
                }
                serviceHost.AddServiceEndpoint(typeof(IMetadataExchange), lBinding, "MEX");
                Log.Debug("MEX endpoint was added.");
            }
            catch (Exception lException)
            {
                Log.Exception(lException);
                throw;
            }
        }
        
        protected override void OnStop()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }
            Log.Info(ServiceName + " was stopped.");
        }
    }

    // Provide the ProjectInstaller class which allows 
    // the service to be installed by the Installutil.exe tool
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            var lProcess = new ServiceProcessInstaller();
            lProcess.Account = ServiceAccount.LocalSystem;

            var lService = new ServiceInstaller();

            lService.ServiceName = Constants.ServiceName;

            Installers.Add(lProcess);
            Installers.Add(lService);
        }
    }
}
