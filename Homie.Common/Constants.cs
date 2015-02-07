namespace Homie.Common
{
    public static class Constants
    {
        public const string AppName = "Homie";
        public const string ServiceName = AppName + "Service";
        public const string ServiceExeName = AppName + "." + "Service";
        public const string ServiceDisplayName = AppName + " Remotedesktop Server";

        public const string WebServiceUrlTemplate = "{0}://{1}:{2}/{3}/";

        public const string HttpPrefix = "http";
        public const string HttpsPrefix = "https";

        public const string MachineControlServiceEndPoint = "IMachineControlService";
        public const string ServiceLogProviderEndPoint = "IServiceLogProvider";
    }
}
