namespace Homie.Model
{
    public interface IServiceSettingsProvider
    {
        int ListenPort { get; set; }

        AuthenticationMode AuthenticationMode { get; set; }

        string CertificateDirectoryName { get; set; }

        string CertificateThumbprint { get; set; }

        string EndPoint { get; set; }

        string Hostname { get; set; }
    }
}
