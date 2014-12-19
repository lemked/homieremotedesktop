using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using MVVMLib.Service;
using MVVMLib.WindowViewModelMapping;

using Homie.Client.View;
using Homie.Client.ViewModel;
using Homie.Client.WindowViewModelMapping;
using Homie.Common;

namespace Homie.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            base.OnStartup(e);

            // Register global event handlers for unhandled exceptions.
            Current.DispatcherUnhandledException += CurrentDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerUnobservedTaskException;

            // Configure service locator
            ServiceLocator.RegisterSingleton<IDialogService, DialogService>();
            ServiceLocator.RegisterSingleton<IWindowViewModelMappings, WindowViewModelMappings>();

            // Create and show main window
            MainWindow view = new MainWindow();
            view.DataContext = new MainWindowViewModel();
            view.Show();
        }

        #region Global Exception Handlers

        static void TaskSchedulerUnobservedTaskException(object pSender, UnobservedTaskExceptionEventArgs pEventArgs)
        {   
            ExceptionUtils.ShowException(pEventArgs.Exception);
            pEventArgs.SetObserved();
            Environment.Exit((int)ExceptionUtils.ExitCodes.UnobservedTaskException);
        }

        static void CurrentDomainUnhandledException(object pSender, UnhandledExceptionEventArgs pEventArgs)
        {
            Exception lException = ExceptionUtils.UnwrapExceptionObject(pEventArgs.ExceptionObject);
            ExceptionUtils.ShowException(lException);
            Environment.Exit((int)ExceptionUtils.ExitCodes.UnhandledException);
        }

        static void CurrentDispatcherUnhandledException(object pSender, DispatcherUnhandledExceptionEventArgs pEventArgs)
        {
            Exception lException = ExceptionUtils.UnwrapExceptionObject(pEventArgs.Exception);
            ExceptionUtils.ShowException(lException);
            pEventArgs.Handled = true;
            Current.Shutdown((int)ExceptionUtils.ExitCodes.DispatcherUnhandledException);
        }

        #endregion
    }
}
