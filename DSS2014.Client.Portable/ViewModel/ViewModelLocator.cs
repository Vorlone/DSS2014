using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS2014.Client.Portable.ViewModel
{
    public class ViewModelLocator
    {
        public const string CustomerDetailPageKey = "CustomerDetailPage";
        public const string CustomerEditPageKey = "CustomerEditPage";
        public const string CustomersReloadNotification = "CustomersReloadNotification";

        public CustomersViewModel CustomersViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstance<CustomersViewModel>();
            }
        }

        public CustomerDetailViewModel CustomerDetailViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstance<CustomerDetailViewModel>();
            }
        }

        public CustomerEditViewModel CustomerEditViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstance<CustomerEditViewModel>();
            }
        }
    }
}
