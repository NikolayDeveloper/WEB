using System.IO;

namespace Iserv.Niis.Utils.Abstractions
{
    public interface IGenerateHash
    {
        string GenerateFileHash(byte[] bytes);
        string GenerateFileHash(FileStream stream);
    }
}