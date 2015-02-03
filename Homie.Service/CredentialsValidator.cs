using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.ServiceModel;

namespace Homie.Service
{
    class CredentialsValidator: UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            if (userName == null || password == null)
            {
                throw new ArgumentNullException();
            }

            throw new NotImplementedException();

//            if (!(userName == "admin" && password == "admin"))
//            {
//                throw new FaultException("Password or name is wrong");
//            }
        }
    }
}
