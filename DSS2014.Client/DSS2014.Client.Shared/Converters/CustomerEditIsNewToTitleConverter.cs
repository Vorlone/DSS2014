using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Data;

namespace DSS2014.Client.Converters
{
    public class CustomerEditIsNewToTitleConverter : IValueConverter
    {
        private ResourceLoader _resLoader = new ResourceLoader();

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool)
            {
                if ((bool)value)
                    return _resLoader.GetString("CustomerEditTitleNew");
                else
                    return _resLoader.GetString("CustomerEditTitleEdit");
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
