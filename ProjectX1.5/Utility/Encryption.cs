using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace ProjectX1._5.Utility
{
    public class Encryption
    {
        public const int SALT_SIZE = 24;
        public const int HASH_SIZE = 24;
        public const int PBKDF2_ITT = 500;

        public static string CreateHash(string str)
        {
            //generate random salt
            RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SALT_SIZE];
            csprng.GetBytes(salt);

            //generate the strHash
            byte[] hash = PBKDF2(str, salt, PBKDF2_ITT,HASH_SIZE);

            return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
        }

        private static byte[] PBKDF2(string str, byte[] salt, int pBKDF2_ITT, int outputBytes)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(str, salt);
            pbkdf2.IterationCount = pBKDF2_ITT;

            return pbkdf2.GetBytes(outputBytes);
        }

        private static Boolean SlowEquals(byte[] dbHash, byte[] strHash)
        {
            uint diff = (uint)dbHash.Length ^ (uint)strHash.Length;
            
            for(int i=0;i<dbHash.Length && i<strHash.Length;i++)
                diff |= (uint)dbHash[i] ^ (uint)strHash[i];

            return diff == 0;
        }

        public static Boolean ValidateString(string str, string dbHash)
        {
            char[] delimiter = { ':' };
            string[] split = dbHash.Split(delimiter);

            byte[] salt = Convert.FromBase64String(split[0]);
            byte[] hash = Convert.FromBase64String(split[1]);

            byte[] hashValidate = PBKDF2(str, salt, PBKDF2_ITT, hash.Length);

            return SlowEquals(hash, hashValidate);
        }
    }
}