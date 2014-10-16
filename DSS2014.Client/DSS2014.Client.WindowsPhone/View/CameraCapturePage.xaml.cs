using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace DSS2014.Client.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CameraCapturePage : Page
    {
        public CameraCapturePage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            CameraPreview.Stretch = Stretch.UniformToFill;

            _mediaCapture = new MediaCapture();
            await _mediaCapture.InitializeAsync();

            _mediaCapture.SetPreviewRotation(VideoRotation.Clockwise90Degrees);

            CameraPreview.Source = _mediaCapture;
            await _mediaCapture.StartPreviewAsync();

            HardwareButtons.CameraHalfPressed += HardwareButtons_CameraHalfPressed;
            HardwareButtons.CameraPressed += HardwareButtons_CameraPressed;
        }

        protected async override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            await _mediaCapture.StopPreviewAsync();
            _mediaCapture.Dispose();

            HardwareButtons.CameraHalfPressed -= HardwareButtons_CameraHalfPressed;
            HardwareButtons.CameraPressed -= HardwareButtons_CameraPressed;

            base.OnNavigatingFrom(e);
        }

        private MediaCapture _mediaCapture;

        private async void HardwareButtons_CameraPressed(object sender, CameraEventArgs e)
        {
            await CapturePhoto();
        }

        private async void HardwareButtons_CameraHalfPressed(object sender, CameraEventArgs e)
        {
            await _mediaCapture.VideoDeviceController.FocusControl.FocusAsync();
        }

        private async Task CapturePhoto()
        {
            var imageEncodingProperties = ImageEncodingProperties.CreateJpeg();
            var photoStream = new InMemoryRandomAccessStream();

            await _mediaCapture.CapturePhotoToStreamAsync(imageEncodingProperties, photoStream);

            if (PhotoCaptured != null)
                PhotoCaptured.Invoke(this, photoStream);

            Frame.GoBack();
        }

        public event EventHandler<IRandomAccessStream> PhotoCaptured;

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            CapturePhoto();
        }
    }
}
