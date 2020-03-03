using System;
using System.IO;
using System.Security.Cryptography;
using Iserv.Niis.Utils.Abstractions;

namespace Iserv.Niis.Utils.Implementations
{
    public class GenerateHash : IGenerateHash
    {
        public string GenerateFileHash(FileStream stream)
        {
            return BitConverter.ToString(MD5.Create().ComputeHash(stream)).Replace("-", "").ToLower();
        }

        public string GenerateFileHash(byte[] bytes)
        {
            return BitConverter.ToString(MD5.Create().ComputeHash(bytes)).Replace("-", "").ToLower();
        }
    }
}