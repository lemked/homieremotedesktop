using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Homie.Model.Properties;
using System.Runtime.Serialization;

namespace Homie.Model
{
    public class Machines : IEnumerable<Machine>
    {
        private List<Machine> m_List = new List<Machine>();

        public List<Machine> List
        {
            get { return m_List; }
            set { m_List = value; }
        }

        public int Count
        {
            get { return m_List.Count; }
        }

        public IEnumerator<Machine> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Add(Machine pMachine)
        {
            // Temporarily save list count so we can 
            // check if item was successfully added.
            int lListCount = this.List.Count;
            List.Add(pMachine);
            if (this.List.Count == lListCount + 1)
            {
                return true;
            }
            return false;
        }

        public bool Remove(Machine pMachine)
        {
            foreach (var lMachine in m_List)
            {
                if (lMachine.Equals(pMachine))
                {
                    m_List.Remove(lMachine);
                    return true;
                }
            }
            return false;
        }
    }

    [DataContract]
    public class Machine : IDataErrorInfo
    {
        #region Properties

        [DataMember]
        public int MachineID { get; set; }

        [DataMember]
        public string NameOrAddress  { get; set; }

        [DataMember]
        public string MacAddress { get; set; }

        [DataMember]
        public int Port { get; set; }

        #endregion Properties

        #region Creation

        public Machine()
        {
            // required for serialization
        }

        public Machine(string pNameOrAddress, string pMacAddress)
        {
            this.NameOrAddress = pNameOrAddress;
            this.MacAddress = pMacAddress;
        }

        public Machine(string pNameOrAddress, string pMacAddress, int pPort) : this(pNameOrAddress, pMacAddress)
        {
            this.Port = pPort;
        }

        #endregion Creation

        #region Validation

        /// <summary>
        /// Returns true if this object has no validation errors.
        /// </summary>
        public bool IsValid
        {
            //TODO: Check if port is valid
            get
            {
                foreach (string lProperty in m_ValidatedProperties)
                {
                    if (GetValidationError(lProperty) != null)
                        return false;
                }
                return true;
            }
        }

        static readonly string[] m_ValidatedProperties = 
        { 
            "HostNameOrAddress", 
            "MacAddress"
        };

        string GetValidationError(string pPropertyName)
        {
            if (Array.IndexOf(m_ValidatedProperties, pPropertyName) < 0)
                return null;

            string error = null;

            switch (pPropertyName)
            {
                case "HostNameOrAddress":
                    error = this.ValidateHostNameOrAddress();
                    break;

                case "MacAddress":
                    error = this.ValidateMacAddress();
                    break;

                default:
                    Debug.Fail("Unexpected property being validated on Machine: " + pPropertyName);
                    break;
            }

            return error;
        }

        private string ValidateHostNameOrAddress()
        {
            if (!IsValidHostNameOrAddress(this.NameOrAddress))
            {
                return String.Format(Resources.ErrorInvalidHostnameOrIpAddress, this.NameOrAddress);
            }

            return null;
        }

        private bool IsValidHostNameOrAddress(string pHostNameOrAdress)
        {
            Regex lValidIpAddressRegex = new Regex(@"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$");
            Regex lValidHostnameRegex = new Regex(@"^(([a-zA-Z]|[a-zA-Z][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z]|[A-Za-z][A-Za-z0-9\-]*[A-Za-z0-9])$");

            return lValidIpAddressRegex.IsMatch(pHostNameOrAdress) || lValidHostnameRegex.IsMatch(pHostNameOrAdress);
        }

        private string ValidateMacAddress()
        {
            if (!IsValidMacAddress(this.MacAddress))
            {
                return String.Format(Resources.ErrorInvalidMacAddress, this.MacAddress);
            }

            return null;
        }

        private bool IsValidMacAddress(string pMacAddress)
        {
            // There are two valid MAC address format accepted:
            // Example 1: 01-23-45-67-89-ab (default)
            // Example 2: 01:23:45:67:89:ab

            Regex lColonSeparatedMacAddressRegex = new Regex(@"((([a-f]|[0-9]|[A-F]){2}\:){5}([a-f]|[0-9]|[A-F]){2}\b)");
            Regex lHyphenSeparatedMacAddressRegex = new Regex(@"((([a-f]|[0-9]|[A-F]){2}\-){5}([a-f]|[0-9]|[A-F]){2}\b)");

            return lColonSeparatedMacAddressRegex.IsMatch(pMacAddress) || lHyphenSeparatedMacAddressRegex.IsMatch(pMacAddress);
        }

        //private bool IsValidPort(int pPort)
        //{
        //    return pPort >= IPEndPoint.MinPort && pPort <= IPEndPoint.MaxPort;
        //}

        #endregion

        #region IDataErrorInfo Members

        string IDataErrorInfo.Error { get { return null; } }

        string IDataErrorInfo.this[string pPropertyName]
        {
            get { return this.GetValidationError(pPropertyName); }
        }

        #endregion IDataErrorInfo Members
    }
}
