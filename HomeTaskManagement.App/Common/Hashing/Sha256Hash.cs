using System;
using System.Security.Cryptography;
using System.Text;

namespace HomeTaskManagement.App.Common
{
    public sealed class Sha256Hash
    {
        private readonly string _value;

        public Sha256Hash(string value)
        {
            _value = value;
        }

        public static implicit operator string(Sha256Hash hash)
        {
            return Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(hash._value)));
        }
    }
}
