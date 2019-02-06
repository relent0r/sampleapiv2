using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Revera.Stnd.Common.HashAlgorithm
{
    public class PBKDF2WithHMACSHA512
    {
        public static string ComputeHash(string password,
             string hexedSalt,
             int iterations = 100,
             int derivedKeyLength = 32)
        {
            var enc = Encoding.GetEncoding(Encoding.UTF8.CodePage,
               new EncoderExceptionFallback(),
               new DecoderExceptionFallback());
            return HexStringFromBytes(PBKDF2(enc.GetBytes(password), HexStringToByteArray(hexedSalt), iterations, derivedKeyLength));
        }
        public static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte byteValue in bytes)
            {
                var hex = byteValue.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }
        public static byte[] HexStringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        //Returns 32-bit salt
        public static string GetNewHexedSalt()
        {
            var salt = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
                return HexStringFromBytes(salt);
            }

        }

        static byte[] PBKDF2(byte[] password,
            byte[] salt,
            int iterations,
            int derivedKeyLength)
        {

            var hashLength = 0;

            using (var hmac = new HMACSHA512())
            {
                hashLength = hmac.HashSize / 8;
            }

            //Our number of blocks
            var length = (int)Math.Ceiling(
                ((decimal)derivedKeyLength) / ((decimal)hashLength));

            var derivedKey = new byte[derivedKeyLength];
            using (var ms = new MemoryStream())
            {
                for (int blockIndex = 1; blockIndex <= length; blockIndex++)
                {
                    var block = CalculateBlock(password, salt,
                        iterations, blockIndex);
                    ms.Write(block, 0, block.Length);
                }
                ms.SetLength(derivedKeyLength);
                return ms.ToArray();
            }

        }

        static byte[] CalculateBlock(byte[] password,
            byte[] salt,
            int iterations,
            int blockIndex)
        {
            var blockBytes = BitConverter.GetBytes(blockIndex);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(blockBytes, 0, blockBytes.Length);

            byte[] u1;
            using (var hmac = new HMACSHA512(password))
            {
                var firstInput = new byte[salt.Length + blockBytes.Length];
                Array.Copy(salt, firstInput, salt.Length);
                Array.Copy(blockBytes, 0, firstInput,
                    salt.Length, blockBytes.Length);
                u1 = hmac.ComputeHash(firstInput);
            }

            byte[] lastvalue = u1;
            byte[] newvalue;
            byte[] result = u1;
            for (int index = 1; index < iterations; index++)
            {
                using (var hmac = new HMACSHA512(password))
                {
                    newvalue = hmac.ComputeHash(lastvalue);
                    for (int innerIndex = 0; innerIndex < newvalue.Length; innerIndex++)
                        result[innerIndex] = (byte)(newvalue[innerIndex] ^ result[innerIndex]);
                    lastvalue = newvalue;
                }
            }
            return result;
        }
    }
}
