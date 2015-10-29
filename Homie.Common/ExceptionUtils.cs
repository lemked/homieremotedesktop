using System;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace Homie.Common
{
    public class ExceptionUtils
    {
        public enum ExitCodes
        {
            Ok = 0,
            UnhandledException = 91,
            UnobservedTaskException = 92,
            DispatcherUnhandledException = 93
        }

        public static Exception UnwrapExceptionObject(object pException)
        {
            var lException = (Exception)pException;

            if (lException is TargetInvocationException && lException.InnerException is AggregateException)
            {
                return lException.InnerException;
            }
            return lException;
        }

        public static void ShowException(Exception pException)
        {
            var exception = UnwrapExceptionObject(pException);

            if (!Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action) (() =>
                {
                    ShowExceptionMessageDialog(exception.Message);
                }));
            }
            else
            {
                ShowExceptionMessageDialog(exception.Message);
            }
        }

        private static void ShowExceptionMessageDialog(string errorMessage)
        {
            MessageBox.Show
            (
                $"Unexpected error: {errorMessage}", 
                Application.Current.MainWindow.GetType().Assembly.GetName().Name, 
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
        }
    }
}
