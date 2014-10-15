using DSS2014.Client.Portable.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS2014.Client.Portable.ViewModel
{
    public class CustomerDetailViewModel : AsyncViewModelBase
    {
        private INavigationService _navigationService;

        public RelayCommand EditCommand { get; private set; }
        public RelayCommand<Customer> InitCommand { get; private set; }

        private Customer _customer;
        public Customer Customer
        {
            get { return _customer; }
            set { _customer = value; RaisePropertyChanged(); }
        }

        public CustomerDetailViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            InitCommand = new RelayCommand<Customer>(Init);
            EditCommand = new RelayCommand(Edit);
        }

        private void Init(Customer customer)
        {
            Customer = customer;
        }

        private void Edit()
        {
            _navigationService.NavigateTo(ViewModelLocator.CustomerEditPageKey, _customer);
        }
    }
}
