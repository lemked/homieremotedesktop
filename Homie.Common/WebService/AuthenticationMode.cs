namespace Homie.Common.WebService
{
    /// <summary>
    /// Defines the available authentication modes.
    /// </summary>
    public enum AuthenticationMode
    {
        /// <summary>
        /// No authentication.
        /// </summary>
        None,

        /// <summary>
        /// Authentication via credentials (username / password).
        /// </summary>
        Credentials,

        /// <summary>
        /// Certificate based authentication.
        /// </summary>
        Certificate,

        /// <summary>
        /// Certificate based authentication with credentials.
        /// </summary>
        CertificateAndCredentials
    }
}
