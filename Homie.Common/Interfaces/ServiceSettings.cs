using Homie.Model;

namespace Homie.Common.Interfaces
{
    public class ServiceSettings
    {
        public int ListenPort { get; set; }

        public AuthenticationMode AuthenticationMode { get; set; }

        public string CertificateDirectoryName { get; set; }

        public string CertificateThumbprint { get; set; }

        public string EndPoint { get; set; }

        public string Hostname { get; set; }
    }
}
