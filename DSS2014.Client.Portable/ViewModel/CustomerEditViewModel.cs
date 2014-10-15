using DSS2014.Client.Portable.Model;
using DSS2014.Client.Portable.Service;
using GalaSoft.MvvmLight.Command;
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
        public RelayCommand<Customer> InitCommand { get; private set; }

        private Customer _customer;
        public Customer Customer
        {
            get { return _customer; }
            set { _customer = value; RaisePropertyChanged(); }
        }

        public CustomerEditViewModel(INavigationService navigationService, IDataService dataService, IDialogService dialogService)
        {
            _dataService = dataService;
            _dialogService = dialogService;
            _navigationService = navigationService;
            InitCommand = new RelayCommand<Customer>(Init);
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Init(Customer customer)
        {
            if (customer != null)
                Customer = customer;
            else
                Customer = new Customer();
        }

        private async void Save()
        {
            IsLoading = true;

            if (_customer.Id == Guid.Empty)
                await SaveNewCustomer();
            else
                await SaveExistingCustomer();

            IsLoading = false;
        }

        private async Task SaveNewCustomer()
        {
            var result = await _dataService.CreateCustomerAsync(_customer);
            if (result.IsSuccessStatusCode)
            {
                Customer = null;
                _navigationService.GoBack();
            }
            else
            {
                await _dialogService.ShowError("[Fehler beim Erstellen.]", "[Fehler]", "[OK]", null);
            }
        }

        private async Task SaveExistingCustomer()
        { 

        }

        private void Cancel()
        {
            Customer = null;
            _navigationService.GoBack();
        }
    }
}
