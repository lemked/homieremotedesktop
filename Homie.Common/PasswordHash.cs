using System;
using System.Security.Cryptography;

namespace Homie.Common
{
    /// <summary>
    /// Provides password hashing.
    /// </summary>
    /// <see cref="http://csharptest.net/470/another-example-of-how-to-store-a-salted-password-hash/"/>
    public sealed class PasswordHash
    {
        const int SaltSize = 16, HashSize = 20, HashIter = 10000;
        readonly byte[] salt, hash;
        public PasswordHash(string password)
        {
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);
            hash = new Rfc2898DeriveBytes(password, salt, HashIter).GetBytes(HashSize);
        }
        public PasswordHash(byte[] hashBytes)
        {
            Array.Copy(hashBytes, 0, salt = new byte[SaltSize], 0, SaltSize);
            Array.Copy(hashBytes, SaltSize, hash = new byte[HashSize], 0, HashSize);
        }
        public PasswordHash(byte[] salt, byte[] hash)
        {
            Array.Copy(salt, 0, this.salt = new byte[SaltSize], 0, SaltSize);
            Array.Copy(hash, 0, this.hash = new byte[HashSize], 0, HashSize);
        }
        public byte[] ToArray()
        {
            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);
            return hashBytes;
        }
        public byte[] Salt { get { return (byte[])salt.Clone(); } }
        public byte[] Hash { get { return (byte[])hash.Clone(); } }
        public bool Verify(string password)
        {
            byte[] test = new Rfc2898DeriveBytes(password, salt, HashIter).GetBytes(HashSize);
            for (int i = 0; i < HashSize; i++)
            {
                if (test[i] != hash[i])
                {
                    return false;                
                }
            }

            return true;
        }
    }
}
