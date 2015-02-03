namespace Homie.Common
{
    public static class Constants
    {
        public const string AppName = "Homie";
        public const string ServiceName = AppName + "Service";
        public const string ServiceExeName = AppName + "." + "Service";
        public const string ServiceDisplayName = AppName + " Remotedesktop Server";

        public const string WebServiceUrlTemplate = "https://{0}:{1}/{2}/";

        public const string MachineControlServiceEndPoint = "IMachineControlService";
        public const string ServiceLogProviderEndPoint = "IServiceLogProvider";
    }
}
