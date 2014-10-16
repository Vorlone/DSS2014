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
    public sealed partial class BladeFrame : UserControl
    {
        public BladeFrame()
        {
            this.InitializeComponent();
        }

        public bool CanGoBack
        {
            get
            {
                return _navigationHistory.Any();
            }
        }

        private List<Blade> _navigationHistory = new List<Blade>();
        public List<Blade> NavigationHistory
        {
            get { return _navigationHistory; }
        }

        private Blade _currentPage = null;
        public Blade CurrentPage
        {
            get { return _currentPage; }
        }

        public void GoBack()
        {
            if (CanGoBack)
            {
                BladeHost.Children.Remove(_currentPage);
                _currentPage = _navigationHistory.Last();
                _navigationHistory.Remove(_currentPage);
            }
        }

        public void NavigateTo(Type pageType, object parameter)
        {
            var focussedElement = FocusManager.GetFocusedElement();

            if (_currentPage != null && pageType == _currentPage.GetType())
                GoBack();

            var firstElement = _navigationHistory.Where(e => e.GetType() == pageType).FirstOrDefault();
            if (firstElement != null)
            {
                var firstElementIndex = _navigationHistory.IndexOf(firstElement);
                while (_navigationHistory.Count > firstElementIndex)
                {
                    var element = _navigationHistory[_navigationHistory.Count - 1];
                    BladeHost.Children.Remove(element);
                    _navigationHistory.Remove(element);
                }
                GoBack();
            }

            if (_currentPage != null)
                _navigationHistory.Add(_currentPage);

            var bladeInstance = Activator.CreateInstance(pageType) as Blade;
            bladeInstance.Frame = this;
            _currentPage = bladeInstance;
            BladeHost.Children.Add(bladeInstance);

            if (Navigated != null)
                Navigated.Invoke(this, parameter);
        }

        public void NavigateTo(Type pageType)
        {
            NavigateTo(pageType, null);
        }

        public delegate void BladeNavigatedEventHandler(object sender, object parameter);
        public event BladeNavigatedEventHandler Navigated;

    }
}
