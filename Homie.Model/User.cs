using System.Runtime.Serialization;

namespace Homie.Model
{
    [DataContract]
    public class User
    {
        public int ID { get; set; }

        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        //TODO: Add permissions/roles
    }
}