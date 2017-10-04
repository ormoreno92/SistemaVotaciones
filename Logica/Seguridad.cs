using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class Seguridad
    {
        public string SerializarPassword(string password)
        {
            try
            {
                var encodedPassword2 = new UTF8Encoding().GetBytes(password);
                var hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword2);
                var encoded = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
                return encoded;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
    }
}
