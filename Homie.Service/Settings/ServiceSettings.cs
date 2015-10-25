using Homie.Model;

namespace Homie.Service.Settings
{
    public class ServiceSettings
    {
        public int ListenPort { get; set; }

        public AuthenticationMode AuthenticationMode { get; set; }

        public string CertificateFilePath { get; set; }

        public string EndPoint { get; set; }
    }
}
