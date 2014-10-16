using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS2014.Client.Portable.Service
{
    public interface ICameraService
    {
        Task<byte[]> CapturePhotoAsync();
    }
}
