using System;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;
using Homie.Common;
using Homie.Common.Interfaces;
using Homie.Common.Logging;
using Homie.Model;
using Homie.Model.Logging;
using Homie.Service.Settings;
using Ninject;

namespace Homie.Service
{
    public partial class ServiceControl : ServiceBase
    {
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
            var kernel = new StandardKernel();
            kernel.Bind<IServiceLogDataSource>().To<DbServiceLogDataSource>();
            kernel.Bind<IServiceLogProvider>().To<ServiceLogProvider>();
            kernel.Bind<IMachineControlService>().To<MachineControlService>();
            kernel.Bind<IUserControlService>().To<UserControlService>();
            kernel.Bind<IServiceSettingsProvider>().To<DbServiceSettingsProvider>();
            kernel.Bind<IMachineDataSource>().To<DbMachineDataSource>();
            kernel.Bind<IUserDataSource>().To<DbUserDataSource>();

            // Configure default logger
            ILogger textLogger = new FileLogger();
            textLogger.LogLevel = LogLevel.Trace;
            Log.Register(textLogger);

            // The service control object
            var serviceControl = kernel.Get<ServiceControl>();

            foreach (string lArgument in arguments)
            {
                if (lArgument.Equals(NoServiceArgument, StringComparison.InvariantCultureIgnoreCase))
                {
                    RunInConsoleMode(serviceControl);
                    return;
                }

                if (lArgument.Equals(InstallServiceArgument, StringComparison.InvariantCultureIgnoreCase))
                {
                    ManagedInstallerClass.InstallHelper(new[] { Assembly.GetExecutingAssembly().Location });
                    return;
                }
            }

            Run(serviceControl);
        }

        private static void TaskSchedulerUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Log.Exception(e.Exception);
        }

        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception lException = ExceptionUtils.UnwrapExceptionObject(e.ExceptionObject);
            Log.Exception(lException);
            Environment.Exit((int)ExceptionUtils.ExitCodes.UnhandledException);
        }

        private static void RunInConsoleMode(ServiceControl serviceControl)
        {
            // Configure console logger
            ILogger consoleLogger = new ConsoleLogger();
            consoleLogger.LogLevel = LogLevel.Debug;
            Log.Register(consoleLogger);

            Log.Info(Resources.Properties.Resources.ServiceStartedWith, NoServiceArgument);
            serviceControl.OnStart(new string[0]);

            Console.WriteLine(@"Service started, press any key to finish execution.");
            Console.ReadKey();
            serviceControl.OnStop();
        }
    }
}
