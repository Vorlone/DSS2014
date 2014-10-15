using DSS2014.Client.View;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS2014.Client.Service
{
    public class BladeNavigationService : INavigationService
    {
        private Dictionary<String, Type> _configuration = new Dictionary<string, Type>();
        private BladeFrame _frame = null;

        public string CurrentPageKey
        {
            get 
            {
                if (_frame != null)
                {
                    var currentPageConfiguration = _configuration.Where(e => e.Value == _frame.CurrentPage.GetType()).FirstOrDefault();
                    return currentPageConfiguration.Key;
                }

                return null;
            }
        }

        public void ConfigureFrame(BladeFrame frame)
        {
            _frame = frame;
        }

        public void Configure(string pageKey, Type pageType)
        {
            _configuration.Add(pageKey, pageType);
        }

        public void GoBack()
        {
            if (_frame != null)
                _frame.GoBack();
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            if (_frame != null && _configuration.ContainsKey(pageKey))
                _frame.NavigateTo(_configuration[pageKey], parameter);
        }

        public void NavigateTo(string pageKey)
        {
            if (_frame != null && _configuration.ContainsKey(pageKey))
                _frame.NavigateTo(_configuration[pageKey]);
        }
    }
}
