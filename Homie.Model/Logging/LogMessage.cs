using System;
using System.Runtime.Serialization;

namespace Homie.Model.Logging
{
    [DataContract]
    public class LogMessage
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Text { get; set; }
        
        [DataMember]
        public LogLevel Level { get; set; }

        [DataMember]
        public DateTime LogTime { get; set; }

        [DataMember]
        public Object Data { get; set; }
    }
}
