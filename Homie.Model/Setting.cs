using System.Runtime.Serialization;

namespace Homie.Model
{
    [DataContract]
    public class Setting : IEntityWithId
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public string Value { get; set; }

        public Setting()
        {
            // required for serialization
        }

        public Setting(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
