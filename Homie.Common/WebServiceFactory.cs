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

            var lBinding = new BasicHttpsBinding(BasicHttpsSecurityMode.TransportWithMessageCredential);
            lBinding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;

            var lFactory = new ChannelFactory<T>(lBinding, lAddress);

            return lFactory.CreateChannel();
        }
    }
}
