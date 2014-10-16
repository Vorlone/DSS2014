using DSS2014.Client.Portable.Model;
using DSS2014.Client.Portable.Service;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS2014.Client.Portable.ViewModel
{
    public class CustomersViewModel : AsyncViewModelBase
    {
        private IDataService _dataService;
        private IDialogService _dialogService;
        private INavigationService _navigationService;
        private IResourceService _resourceService;

        public RelayCommand NewCommand { get; private set; }
        public RelayCommand RefreshCommand { get; private set; }
        public RelayCommand<CustomerViewModel> ShowCustomerDetailCommand { get; private set; }

        private List<CustomerViewModel> _customers;
        public List<CustomerViewModel> Customers
        {
            get { return _customers; }
            set { _customers = value; RaisePropertyChanged(); }
        }

        public CustomersViewModel(IDataService dataService, IDialogService dialogService, INavigationService navigationService, IResourceService resourceService)
        {
            _dataService = dataService;
            _dialogService = dialogService;
            _navigationService = navigationService;
            _resourceService = resourceService;

            NewCommand = new RelayCommand(New);
            RefreshCommand = new RelayCommand(Refresh);
            ShowCustomerDetailCommand = new RelayCommand<CustomerViewModel>(ShowCustomerDetail);

            Messenger.Default.Register<NotificationMessage<CustomerViewModel>>(this, NotificationReceived);

            LoadCustomers();
        }

        private async void LoadCustomers()
        {
            IsLoading = true;
            LoadingMessage = _resourceService.GetString("CustomersLoadingMessage");

            var result = await _dataService.GetCustomersAsync();
            if (result.IsSuccessStatusCode && result.Result != null)
            {
                Customers = (from c in result.Result.OrderBy(e => e.FirstName)
                             select new CustomerViewModel(c, _dataService)).ToList();
            }
            else
            {
                await _dialogService.ShowError(_resourceService.GetString("CustomersLoadingErrorMessage"), _resourceService.GetString("CustomersLoadingErrorTitle"), _resourceService.GetString("CustomerLoadingErrorButton"), null);
            }

            LoadingMessage = String.Empty;
            IsLoading = false;
        }

        private void ShowCustomerDetail(CustomerViewModel customer)
        {
            _navigationService.NavigateTo(ViewModelLocator.CustomerDetailPageKey, customer);
        }

        private void Refresh()
        {
            LoadCustomers();
        }

        private void New()
        {
            _navigationService.NavigateTo(ViewModelLocator.CustomerEditPageKey);
        }

        private void NotificationReceived(NotificationMessage<CustomerViewModel> message)
        {
            if (message.Notification == ViewModelLocator.CustomersReloadNotification)
                LoadCustomers();
        }
    }
}
