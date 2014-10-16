using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS2014.Client.Portable.Service
{
    public interface ILocalStorageService
    {
        Task<String> ReadStringFromStorageAsync(string fileName);

        Task WriteStringToStorageAsync(string fileName, string content);

        Task<byte[]> ReadBinaryFromStorageAsync(string fileName);

        Task WriteBinaryToStorageAsync(string fileName, byte[] data);
    }
}
