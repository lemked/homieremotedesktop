using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Homie.Common.Interfaces;
using Homie.Model;

namespace Homie.Common.WebService
{
    /// <summary>
    /// Factory class for web services.
    /// </summary>
    public class WebServiceFactory
    {
        public Binding Binding { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
        public string ServiceEndPoint { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public WebServiceFactory(Binding binding, string address, int port, string serviceEndPoint)
        {
            this.Binding = binding;
            this.Address = address;
            this.Port = port;
            this.ServiceEndPoint = serviceEndPoint;
        }

        public WebServiceFactory(Binding binding, string address, int port, string serviceEndPoint, string username, string password)
            : this(binding, address, port, serviceEndPoint)
        {
            this.Username = username;
            this.Password = password;
        }

        public T Create<T>()
        {
            // Determine the protocol of the web service
            var protocol = Protocol.Http; // default
            if (Binding is BasicHttpsBinding)
            {
                protocol = Protocol.Https;
            }
            else if (Binding is NetTcpBinding)
            {
                protocol = Protocol.NetTcp;
            }

            var endPointUrl = String.Format(Constants.WebServiceUrlTemplate, protocol, Address, Port, ServiceEndPoint);

            if (typeof(T) == typeof(IMachineControlService))
            {
                endPointUrl = endPointUrl + Constants.MachineControlServiceEndPoint;
            }
            if (typeof(T) == typeof(IUserControlService))
            {
                endPointUrl = endPointUrl + Constants.UserControlServiceEndPoint;
            }
            if (typeof(T) == typeof(IServiceLogProvider))
            {
                endPointUrl = endPointUrl + Constants.ServiceLogProviderEndPoint;
            }

            var lAddress = new EndpointAddress(new Uri(endPointUrl));

            var lFactory = new ChannelFactory<T>(Binding, lAddress);

            if (lFactory.Credentials != null && !string.IsNullOrEmpty(this.Username) && !string.IsNullOrEmpty(Password))
            {
                lFactory.Credentials.UserName.UserName = Username;
                lFactory.Credentials.UserName.Password = Password;
            }

            return lFactory.CreateChannel();
        }
    }
}
