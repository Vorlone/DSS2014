using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace DSS2014.Client.View
{
    public abstract class Blade : UserControl
    {
        private BladeFrame _frame;
        public BladeFrame Frame
        {
            get { return _frame; }
            set { _frame = value; RegisterNavigationEvents(); }
        }

        private void RegisterNavigationEvents()
        {
            if (_frame != null)
            {
                _frame.Navigated -= _frame_Navigated;
                _frame.Navigated += _frame_Navigated;
            }
        }

        public Blade()
        {
            this.Margin = new Windows.UI.Xaml.Thickness(0, 0, 2, 0);
            this.Width = 320.0;
        }

        void _frame_Navigated(object sender, object parameter)
        {
            OnNavigatedTo(parameter);
        }

        protected virtual void OnNavigatedTo(object parameter)
        { }
    }
}
