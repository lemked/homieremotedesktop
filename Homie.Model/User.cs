using System.Runtime.Serialization;

namespace Homie.Model
{
    [DataContract]
    public class User
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public byte[] PasswordHash { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string EmailAddress { get; set; }

        //TODO: Add permissions/roles
    }
}