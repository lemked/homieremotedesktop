namespace Homie.Common
{
    public static class Constants
    {
        public const string AppName = "Homie";
        public const string ServiceName = AppName + "Service";
        public const string ServiceExeName = AppName + "." + "Service";
        public const string ServiceDisplayName = AppName + " Remotedesktop Server";

        public const string SERVICE_URL_TEMPLATE = "net.tcp://{0}:{1}/{2}/";
        public const string SERVICE_URL_END_POINT = "HomieServices";
        public const string MACHINECONTROLSERVICE_URL_END_POINT = "IMachineControlService";
        public const string SERVICELOGREADER_URL_END_POINT = "IServiceLogProvider";

        public const int DEFAULT_PORT = 8730;
    }
}
