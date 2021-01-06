using System;
using System.Security.Cryptography;

namespace Comuni
{
    class Password
    {
        internal static string CreateSalt(int size)
        {
            //Generate a cryptographic random number.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);

            // Return a Base64 string representation of the random number.
            return Convert.ToBase64String(buff);
        }

        internal static string CreatePasswordHash(string GivenPassword, string Salt)
        {
            string saltAndPwd = String.Concat(GivenPassword, Salt);
            byte[] data = System.Text.Encoding.ASCII.GetBytes(saltAndPwd);
            byte[] hashedPwd = System.Security.Cryptography.SHA1.Create().ComputeHash(data);
            return Convert.ToBase64String(hashedPwd);
        }

        internal static bool ComparePasswordWithHash(string GivenPassword, string Salt, string ReadHash)
        {
            string saltAndPwd = String.Concat(GivenPassword, Salt);
            byte[] data = System.Text.Encoding.ASCII.GetBytes(saltAndPwd);
            byte[] hashedPwd = System.Security.Cryptography.SHA1.Create().ComputeHash(data);
            return ReadHash == Convert.ToBase64String(hashedPwd);
        }
    }
}
