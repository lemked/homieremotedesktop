﻿using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.ServiceProcess;
using System.Threading.Tasks;
using Homie.Common;
using Homie.Common.Interfaces;
using Homie.Common.Logging;
using Homie.Common.WebService;
using Homie.Model;
using Homie.Model.Logging;
using Homie.Service.Properties;
using Homie.Service.Settings;

namespace Homie.Service
{
    public class ServiceControl : ServiceBase
    {
        private const string NoServiceArgument = "/NOSERVICE";
        private const string InstallServiceArgument = "/INSTALL";

        private ServiceHost serviceHost;

        private readonly IServiceSettingsProvider serviceSettingsProvider;

        public ServiceControl()
        {
            // Name the Windows Service
            ServiceName = Constants.ServiceName;

            // Configure database logger
            ILogger dbLogger = new DbLogger();
            dbLogger.LogLevel = LogLevel.Info;
            Log.Register(dbLogger);
            
            // Load the service settings 
            serviceSettingsProvider = DependencyInjector.Resolve<IServiceSettingsProvider>();
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
            DependencyInjector.Register<IUserControlService, UserControlService>();
            DependencyInjector.Register<IServiceSettingsProvider, DbServiceSettingsProvider>();


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

            Run(new ServiceControl()); // TODO: Use DI container
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

            Log.Info(Resources.Properties.Resources.ServiceStartedWith, NoServiceArgument);
            var lServiceControl = new ServiceControl();
            lServiceControl.OnStart(new string[0]);

            Console.WriteLine(@"Service started, press any key to finish execution.");
            Console.ReadKey();
            lServiceControl.OnStop();
        }

        // Start the Windows service.
        protected override void OnStart(string[] arguments)
        {
            Log.Info(Resources.Properties.Resources.ServiceIsStarting);

            if (serviceHost != null)
            {
                // Close the service host if an instance already exists
                serviceHost.Close();
            }

            // Create Service Host
            serviceHost = new ServiceHost(typeof(HomieService));
            
            AddServiceEndpoint();
            serviceHost.Open();

            Log.Info(Resources.Properties.Resources.ServiceIsUpAndRunning);
        }

        private void SetCredentialsStore()
        {
            IUserDataSource userDataSource = new DbUserDataSource(); // TODO: Resolve dependency
            serviceHost.Credentials.UserNameAuthentication.UserNamePasswordValidationMode = UserNamePasswordValidationMode.Custom;
            serviceHost.Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = new CredentialsValidator(userDataSource);
        }

        private void AddServiceEndpoint()
        {
            Binding binding;
            ServiceSettings settings = serviceSettingsProvider.GetSettings();

            switch (settings.AuthenticationMode)
            {
                case AuthenticationMode.None:
                    binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                    break;
                case AuthenticationMode.Credentials:
                    binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);
                    SetCredentialsStore();
                    break;
                case AuthenticationMode.Certificate:
                    binding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport);
                    ImportCertifcate();
                    break;
                case AuthenticationMode.CertificateAndCredentials:
                    binding = new BasicHttpsBinding(BasicHttpsSecurityMode.TransportWithMessageCredential);
                    ImportCertifcate();
                    SetCredentialsStore();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(Resources.Properties.Resources.UnsupportedAuthenticationMode);
            }

            var protocol = Protocol.Http;
            if (binding is BasicHttpsBinding)
            {
                protocol = Protocol.Https;
            }

            var baseAddress = new Uri(String.Format(
                Constants.WebServiceUrlTemplate,
                protocol,
                Environment.MachineName,
                settings.ListenPort,
                settings.EndPoint));

            Log.Debug("Adding service endpoints ...");

            var machineServiceEndPoint = baseAddress + Constants.MachineControlServiceEndPoint;
            serviceHost.AddServiceEndpoint(typeof(IMachineControlService), binding, new Uri(machineServiceEndPoint));
            Log.Debug("Service endpoint added: " + machineServiceEndPoint);

            var userServiceEndPoint = baseAddress + Constants.UserControlServiceEndPoint;
            serviceHost.AddServiceEndpoint(typeof(IUserControlService), binding, new Uri(userServiceEndPoint));
            Log.Debug("Service endpoint added: " + userServiceEndPoint);

            var serviceLogProviderEndPoint = baseAddress + Constants.ServiceLogProviderEndPoint;
            serviceHost.AddServiceEndpoint(typeof(IServiceLogProvider), binding, new Uri(serviceLogProviderEndPoint));
            Log.Debug("Service endpoint added: " + serviceLogProviderEndPoint);
        }

        private void ImportCertifcate()
        {
            ServiceSettings settings = serviceSettingsProvider.GetSettings();

            if (!File.Exists(settings.CertificateFilePath))
            {
                throw new SecurityException(
                    Resources.Properties.Resources.No_certificate_found__cannot_establish_a_secure_connection);
            }

            // Load the certificate into an X509Certificate object.
            X509Certificate cert = new X509Certificate();
            cert.Import(settings.CertificateFilePath);

            Log.Info($"Imported certificate \"{cert.Subject}\" from certificate directory.");
        }
        
        protected override void OnStop()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }

            Log.Info(Resources.Properties.Resources.ServiceWasStopped);
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
