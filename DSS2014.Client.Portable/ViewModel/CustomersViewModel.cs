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
    public class CustomersViewModel : AsyncViewModelBase
    {
        private IDataService _dataService;
        private IDialogService _dialogService;
        private INavigationService _navigationService;
        private IResourceService _resourceService;

        public RelayCommand NewCommand { get; private set; }
        public RelayCommand RefreshCommand { get; private set; }
        public RelayCommand<Customer> ShowCustomerDetailCommand { get; private set; }

        private List<Customer> _customers;
        public List<Customer> Customers
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
            ShowCustomerDetailCommand = new RelayCommand<Customer>(ShowCustomerDetail);           

            LoadCustomers();
        }

        private async void LoadCustomers()
        {
            IsLoading = true;
            LoadingMessage = _resourceService.GetString("CustomersLoadingMessage");

            var result = await _dataService.GetCustomersAsync();
            if (result.IsSuccessStatusCode && result.Result != null)
            {
                Customers = result.Result.ToList();
            }
            else
            {
                await _dialogService.ShowError(_resourceService.GetString("CustomersLoadingErrorMessage"), _resourceService.GetString("CustomersLoadingErrorTitle"), _resourceService.GetString("CustomerLoadingErrorButton"), null);
            }

            LoadingMessage = String.Empty;
            IsLoading = false;
        }

        private void ShowCustomerDetail(Customer customer)
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
    }
}
