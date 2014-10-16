using DSS2014.Client.Portable.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.Runtime.InteropServices.WindowsRuntime;

namespace DSS2014.Client.Service
{
    public class LocalStorageService : ILocalStorageService
    {
        private StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;

        public async Task<string> ReadStringFromStorageAsync(string fileName)
        {
            var file = await folder.GetFileAsync(fileName);
            if (file == null)
                return null;
            else
                return await FileIO.ReadTextAsync(file);
        }

        public async Task WriteStringToStorageAsync(string fileName, string content)
        {
            var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, content);
        }

        public async Task<byte[]> ReadBinaryFromStorageAsync(string fileName)
        {
            var file = await folder.GetFileAsync(fileName);
            if (file == null)
                return null;
            else
            {
                var buffer = await FileIO.ReadBufferAsync(file);
                return buffer.ToArray();
            }
        }

        public async Task WriteBinaryToStorageAsync(string fileName, byte[] data)
        {
            var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteBytesAsync(file, data);
        }
    }
}
