using DSS2014.Client.Helper;
using DSS2014.Client.Portable.Service;
using DSS2014.Client.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Media.Capture;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace DSS2014.Client.Service
{
    public class CameraService : ICameraService
    {
        public async Task<byte[]> CapturePhotoAsync()
        {
            byte[] photo = null;
            IRandomAccessStream stream = null;

#if WINDOWS_APP
            var cameraCaptureUI = new CameraCaptureUI();
            var file = await cameraCaptureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);
            stream = await file.OpenReadAsync();
#endif
            
#if WINDOWS_PHONE_APP
            if (((Frame)Window.Current.Content).Navigate(typeof(CameraCapturePage)))
            {
                var page = (CameraCapturePage)((Frame)Window.Current.Content).Content;

                stream = await TaskExt.FromEvent<IRandomAccessStream>(
                    handler=>page.PhotoCaptured += new EventHandler<IRandomAccessStream>(handler),
                    ()=>{},
                    handler => page.PhotoCaptured -= new EventHandler<IRandomAccessStream>(handler),
                    CancellationToken.None);
            }
#endif

            if (stream != null)
            {
                var reader = new DataReader(stream.GetInputStreamAt(0));
                photo = new byte[stream.Size];
                await reader.LoadAsync((uint)stream.Size);
                reader.ReadBytes(photo);
            }

            return photo;
        }
    }
}
