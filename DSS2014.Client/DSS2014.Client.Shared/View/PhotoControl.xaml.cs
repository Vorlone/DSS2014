using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DSS2014.Client.View
{
    public sealed partial class PhotoControl : UserControl
    {
        public PhotoControl()
        {
            this.InitializeComponent();
        }

        #region SourceProperty

        public object Source
        {
            get { return (object)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(object), typeof(PhotoControl), new PropertyMetadata(null, SourceChanged));

        private static void SourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null && e.NewValue is byte[])
            {
                ((PhotoControl)sender).LoadPhoto((byte[])e.NewValue);
            }
        }

        #endregion

        private async void LoadPhoto(byte[] data)
        {
            var bitmapImage = new BitmapImage();

            var stream = new InMemoryRandomAccessStream();
            await stream.WriteAsync(data.AsBuffer());
            stream.Seek(0);

            bitmapImage.SetSource(stream);

            PhotoEllipse.Fill = new ImageBrush() { ImageSource = bitmapImage, Stretch = Stretch.UniformToFill };
        }
    }
}
