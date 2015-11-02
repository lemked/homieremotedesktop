using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Homie.Admin
{
    public class ServiceStatusToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (ServiceControllerStatus)value;

            switch (status)
            {
                case ServiceControllerStatus.ContinuePending:
                    return "Continue pending";
                case ServiceControllerStatus.PausePending:
                    return "Pause pending";
                case ServiceControllerStatus.Paused:
                    return "Paused";
                case ServiceControllerStatus.Running:
                    return "Running";
                case ServiceControllerStatus.StartPending:
                    return "Start pending";
                case ServiceControllerStatus.StopPending:
                    return "Stop pending";
                case ServiceControllerStatus.Stopped:
                    return "Stopped";
                default:
                    return "Unknown";
            }

            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
