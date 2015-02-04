using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using Homie.Common.Interfaces;

namespace Homie.Common
{
    /// <summary>
    /// Factory class for web services.
    /// </summary>
    /// <author>Daniel Lemke - lemked@web.de</author>
    public class WebServiceFactory
    {
        private Binding binding;
        private string address;
        private int port;
        private string serviceEndPoint;
        public WebServiceFactory(Binding binding, string address, int port, string serviceEndPoint)
        {
            this.binding = binding;
            this.address = address;
            this.port = port;
            this.serviceEndPoint = serviceEndPoint;
        }

        public T Create<T>(UserNamePasswordClientCredential credentials = null) where T : IWebServiceSession
        {
            var endPointUrl = String.Format(Constants.WebServiceUrlTemplate, address, port, serviceEndPoint);

            if (typeof(T) == typeof(IMachineControlService))
            {
                endPointUrl = endPointUrl + Constants.MachineControlServiceEndPoint;
            }
            if (typeof(T) == typeof(IServiceLogProvider))
            {
                endPointUrl = endPointUrl + Constants.ServiceLogProviderEndPoint;
            }

            var lAddress = new EndpointAddress(new Uri(endPointUrl));

            var lFactory = new ChannelFactory<T>(binding, lAddress);

            if (lFactory.Credentials != null && credentials != null)
            {
                lFactory.Credentials.UserName.UserName = credentials.UserName;
                lFactory.Credentials.UserName.Password = credentials.Password;
            }

            return lFactory.CreateChannel();
        }
    }
}
