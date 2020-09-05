using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Mochou.Core
{
    public class RSA
    {
        public static String RSAEncrypt(string content, string publickey, Encoding encoding)
        {
            if (encoding == null) {
                encoding = Encoding.ASCII;
            }
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(publickey);
            cipherbytes = rsa.Encrypt(encoding.GetBytes(content), false);
            return encoding.GetString(cipherbytes);
        }
    }
}
