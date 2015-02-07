using System;

namespace Homie.Common.WebService
{
    public enum Protocol
    {
        Http,
        Https,
        NetTcp
    }

    public static class ProtocolsEnumExtension
    {
        public static string EnumValue(this Protocol argument)
        {
            switch (argument)
            {
                case Protocol.Http:
                    return "http";
                case Protocol.Https:
                    return "https";
                case Protocol.NetTcp:;
                    return "net.tcp";
                default:
                    throw new ArgumentOutOfRangeException("Unsupported enum value");
            }
        }
    }
}
