using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;

namespace DSS2014.Client.Helper
{
    public static class VisualTreeHelper
    {
        public static T GetParent<T>(DependencyObject element)
        {
            var parent = Windows.UI.Xaml.Media.VisualTreeHelper.GetParent(element);
            if (parent == null)
                return default(T);

            if (parent is T)
                return (T)(object)parent;

            return GetParent<T>(parent);
        }
    }
}
