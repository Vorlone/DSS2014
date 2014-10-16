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
    public class CustomerEditViewModel : AsyncViewModelBase
    {
        private IDataService _dataService;
        private INavigationService _navigationService;
        private IDialogService _dialogService;

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand<CustomerViewModel> InitCommand { get; private set; }

        private CustomerViewModel _customer;
        public CustomerViewModel Customer
        {
            get { return _customer; }
            set { _customer = value; RaisePropertyChanged(); }
        }

        private bool _isNew = false;
        public bool IsNew
        {
            get { return _isNew; }
            set { _isNew = value; RaisePropertyChanged(); }
        }

        public CustomerEditViewModel(INavigationService navigationService, IDataService dataService, IDialogService dialogService)
        {
            _dataService = dataService;
            _dialogService = dialogService;
            _navigationService = navigationService;
            InitCommand = new RelayCommand<CustomerViewModel>(Init);
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Init(CustomerViewModel customer)
        {
            if (customer != null)
            {
                Customer = customer;
                IsNew = false;
            }
            else
            {
                Customer = new CustomerViewModel(new Customer(), _dataService);
                IsNew = true;
            }
        }

        private async void Save()
        {
            IsLoading = true;

            HttpResponse<Customer> result;

            if (_isNew)
                result = await _dataService.CreateCustomerAsync(_customer.Customer);
            else
                result = await _dataService.EditCustomerAsync(_customer.Customer);

            if (result.IsSuccessStatusCode)
            {
                Customer = null;
                Messenger.Default.Send<NotificationMessage<CustomerViewModel>>(new NotificationMessage<CustomerViewModel>(null, ViewModelLocator.CustomersReloadNotification));
                _navigationService.GoBack();
            }
            else
            {
                await _dialogService.ShowError("[Fehler beim Erstellen.]", "[Fehler]", "[OK]", null);
            }

            IsLoading = false;
        }

        private void Cancel()
        {
            Customer = null;
            _navigationService.GoBack();
        }
    }
}
