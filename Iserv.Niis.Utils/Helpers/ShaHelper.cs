using System.Security.Cryptography;
using System.Text;

namespace Iserv.Niis.Utils.Helpers
{
    public static class ShaHelper
    {
        public static string GenerateSha256String(string inputString)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(inputString);
                var hash = sha256.ComputeHash(bytes);
                return GetStringFromHash(hash);
            }
        }

        public static string GenerateSha512String(string inputString)
        {
            using (var sha512 = SHA512.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(inputString);
                var hash = sha512.ComputeHash(bytes);
                return GetStringFromHash(hash);
            }
        }

        private static string GetStringFromHash(byte[] hash)
        {
            var result = new StringBuilder();
            foreach (var b in hash)
            {
                result.Append(b.ToString("X2"));
            }
            return result.ToString();
        }
    }
}