using System;
using System.ServiceModel;

using Homie.Common.Interfaces;

namespace Homie.Common
{
    /// <summary>
    /// Factory class for web services.
    /// </summary>
    /// <author>Daniel Lemke - lemked@web.de</author>
    public static class WebServiceFactory
    {
        public static T Create<T>(string address, int port, string serviceEndPoint) where T: IWebServiceSession
        {
            var endPointUrl = String.Format(Constants.SERVICE_URL_TEMPLATE, address, port, serviceEndPoint);

            if (typeof(T) == typeof(IMachineControlService))
            {
                endPointUrl = endPointUrl + Constants.MACHINECONTROLSERVICE_URL_END_POINT;
            }
            if (typeof(T) == typeof(IServiceLogProvider))
            {
                endPointUrl = endPointUrl + Constants.SERVICELOGREADER_URL_END_POINT;
            }

            var lAddress = new EndpointAddress(new Uri(endPointUrl));

            var lBinding = new NetTcpBinding(SecurityMode.None);
            var lFactory = new ChannelFactory<T>(lBinding, lAddress);
            return lFactory.CreateChannel();
        }

        public static T Create<T>(string address, int port) where T : IWebServiceSession
        {
            return Create<T>(address, port, Constants.SERVICE_URL_END_POINT);
        }

        public static T Create<T>() where T : IWebServiceSession
        {
            return Create<T>("localhost", Constants.DEFAULT_PORT, Constants.SERVICE_URL_END_POINT);
        }
    }
}
