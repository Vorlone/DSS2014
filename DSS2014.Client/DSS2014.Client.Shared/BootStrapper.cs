using DSS2014.Client.Portable.Service;
using DSS2014.Client.Portable.ViewModel;
using DSS2014.Client.Service;
using DSS2014.Client.View;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSS2014.Client
{
    public class BootStrapper
    {
        public BootStrapper()
        {
            Init();
        }

        private void Init()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

#if WINDOWS_PHONE_APP
            var nav = new NavigationService();
            nav.Configure(ViewModelLocator.CustomerDetailPageKey, typeof(CustomerDetailPage));
            nav.Configure(ViewModelLocator.CustomerEditPageKey, typeof(CustomerEditPage));
            SimpleIoc.Default.Register<INavigationService>(() => nav);
#endif

#if WINDOWS_APP
            var nav = new BladeNavigationService();
            nav.Configure(ViewModelLocator.CustomerDetailPageKey, typeof(CustomerDetailBlade));
            nav.Configure(ViewModelLocator.CustomerEditPageKey, typeof(CustomerEditBlade));
            SimpleIoc.Default.Register<INavigationService>(() => nav);
#endif

            SimpleIoc.Default.Register<IHttpService, HttpService>();
            SimpleIoc.Default.Register<IDataService, DataService>();
            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<IResourceService, ResourceService>();
            SimpleIoc.Default.Register<ICameraService, CameraService>();

            SimpleIoc.Default.Register<CustomersViewModel>();
            SimpleIoc.Default.Register<CustomerDetailViewModel>();
            SimpleIoc.Default.Register<CustomerEditViewModel>();
        }
    }
}
