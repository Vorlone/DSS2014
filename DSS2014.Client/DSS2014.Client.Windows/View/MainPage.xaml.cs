using DSS2014.Client.Service;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DSS2014.Client.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ResourceLoader _resLoader = new ResourceLoader();

        public MainPage()
        {
            this.InitializeComponent();

            TxbTitle.Text = _resLoader.GetString("AppTitle");
        }

        private void BladeFrame_Loaded(object sender, RoutedEventArgs e)
        {
            var bladeNavigationService = SimpleIoc.Default.GetInstance<INavigationService>();
            if (bladeNavigationService != null && bladeNavigationService is BladeNavigationService)
            {
                ((BladeNavigationService)bladeNavigationService).ConfigureFrame((BladeFrame)sender);
                ((BladeFrame)sender).NavigateTo(typeof(CustomersBlade));
            }
        }
    }
}
