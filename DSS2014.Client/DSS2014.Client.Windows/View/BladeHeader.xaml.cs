using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DSS2014.Client.View
{
    public sealed partial class BladeHeader : UserControl
    {
        public BladeHeader()
        {
            this.InitializeComponent();
        }

        #region TitleProperty

        public String Title
        {
            get { return (String)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(String), typeof(BladeHeader), new PropertyMetadata(String.Empty));

        #endregion

        #region HasCloseButtonProperty

        public bool HasCloseButton
        {
            get { return (bool)GetValue(HasCloseButtonProperty); }
            set { SetValue(HasCloseButtonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HasCloseButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasCloseButtonProperty =
            DependencyProperty.Register("HasCloseButton", typeof(bool), typeof(BladeHeader), new PropertyMetadata(true));

        #endregion

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var blade = DSS2014.Client.Helper.VisualTreeHelper.GetParent<Blade>(this);
            var frame = DSS2014.Client.Helper.VisualTreeHelper.GetParent<BladeFrame>(this);
            if (frame != null && blade != null)
                frame.CloseBlade(blade);
        }
    }
}
