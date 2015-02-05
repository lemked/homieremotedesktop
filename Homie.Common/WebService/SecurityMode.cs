namespace Homie.Common.WebService
{
    /// <summary>
    /// Defines the available security modes.
    /// </summary>
    public enum SecurityMode
    {
        /// <summary>
        /// No authentication.
        /// </summary>
        None,
        Credentials,
        Certificate,
        CertificateAndCredentials
    }
}
