using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.ServiceProcess;
using System.Threading.Tasks;

using Homie.Common;
using Homie.Common.Interfaces;
using Homie.Common.Logging;
using Homie.Model.Logging;
using Homie.Service.Properties;

namespace Homie.Service
{
    public class ServiceControl : ServiceBase
    {
        private const string NoServiceArgument = "/NOSERVICE";
        private const string InstallServiceArgument = "/INSTALL";

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
        /// <param name="arguments">The p arguments.</param>
        public static void Main(string[] arguments)
        {
            // Register global event handlers for unhandled exceptions.
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerUnobservedTaskException;

            // Set up depedency injection.
            DependencyInjector.Register<IServiceLogDataSource, DbServiceLogDataSource>();
            DependencyInjector.Register<IServiceLogProvider, ServiceLogProvider>();
            DependencyInjector.Register<IMachineControlService, MachineControlService>();

            // Configure default logger
            ILogger textLogger = new FileLogger();
            textLogger.LogLevel = LogLevel.Trace;
            Log.Register(textLogger);
            
            foreach (string lArgument in arguments)
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
            Environment.Exit((int) ExceptionUtils.ExitCodes.UnhandledException);
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
        protected override void OnStart(string[] arguments)
        {
            Log.Info("Service is starting ...");

            if (serviceHost != null)
            {
                // Close the service host if an instance already exists
                serviceHost.Close();
            }

            // Create Service Host
            serviceHost = new ServiceHost(typeof(HomieService));
            
            AddServiceEndpoint();
            serviceHost.Open();

            Log.Info(ServiceName + " is up and running.");
        }

        private void AddServiceEndpoint()
        {
            var baseAddress = new Uri(String.Format(
                Constants.WebServiceUrlTemplate, 
                Settings.Default.Hostname, 
                Settings.Default.ListenPort, 
                Settings.Default.EndPoint));

            // Create HTTPS Binding with TransportWithMessageCredential security
            var binding = new BasicHttpsBinding(BasicHttpsSecurityMode.TransportWithMessageCredential);

            // Use UserName Message Authentication
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;

            serviceHost.Credentials.UserNameAuthentication.UserNamePasswordValidationMode = UserNamePasswordValidationMode.Custom;
            serviceHost.Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = new CredentialsValidator();

            ImportCertifcate();
                
            Log.Debug("Adding service endpoints ...");

            var machineServiceEndPoint = baseAddress + Constants.MachineControlServiceEndPoint;
            serviceHost.AddServiceEndpoint(typeof(IMachineControlService), binding, new Uri(machineServiceEndPoint));
            Log.Debug("Service endpoint added: " + machineServiceEndPoint);

            var serviceLogProviderEndPoint = baseAddress + Constants.ServiceLogProviderEndPoint;
            serviceHost.AddServiceEndpoint(typeof(IServiceLogProvider), binding, new Uri(serviceLogProviderEndPoint));
            Log.Debug("Service endpoint added: " + serviceLogProviderEndPoint);
        }

        private void ImportCertifcate()
        {
            if (Directory.Exists(Settings.Default.CertificateDirectoryName))
            {
                var certificates = Directory.GetFiles(Settings.Default.CertificateDirectoryName, "*.cer", SearchOption.TopDirectoryOnly);
                if (certificates.Length > 0)
                {
                    // Load the certificate into an X509Certificate object.
                    X509Certificate cert = new X509Certificate();
                    cert.Import(certificates[0]);

                    Log.Info(string.Format("Imported certificate \"{0}\" from certificate directory.", cert.Subject));
                    return;
                }
            }

            // Attach a Certificate from the Certificate Store to the HTTP Binding using the specified Thumbprint
            if (!string.IsNullOrEmpty(Settings.Default.CertificateThumbprint))
            {
                try
                {
                    serviceHost.Credentials.ServiceCertificate.SetCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindByThumbprint, Settings.Default.CertificateThumbprint);
                    Log.Info(string.Format("Imported certificate with Thumbprint \"{0}\" from Certificate Store.", Settings.Default.CertificateThumbprint));
                    return;
                }
                catch (FormatException exception)
                {
                    throw new SecurityException(string.Format("Not a valid certificate thumbprint: \"{0}\"", Settings.Default.CertificateThumbprint), exception);
                }
            }

            throw new SecurityException("No certificate found, cannot establish a secure connection.");
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
