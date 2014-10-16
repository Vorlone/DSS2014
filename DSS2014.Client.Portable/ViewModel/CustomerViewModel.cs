using DSS2014.Client.Portable.Model;
using DSS2014.Client.Portable.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS2014.Client.Portable.ViewModel
{
    public class CustomerViewModel : AsyncViewModelBase
    {
        private IDataService _dataService;

        private Customer _customer;
        public Customer Customer
        {
            get { return _customer; }
            set { _customer = value; RaisePropertyChanged(); }
        }

        private byte[] _photo;
        public byte[] Photo
        {
            get { return _photo; }
            set { _photo = value; RaisePropertyChanged(); }
        }

        public CustomerViewModel(Customer customer, IDataService dataService)
        {
            _dataService = dataService;
            Customer = customer;
            if (!String.IsNullOrEmpty(customer.PhotoUrl))
            {
                LoadPhoto();
            }
        }

        private async void LoadPhoto()
        {
            IsLoading = true;

            var response = await _dataService.GetPhotoAsync(_customer.PhotoUrl);
            if (response.IsSuccessStatusCode && response.Result != null)
            {
                Photo = response.Result;
            }

            IsLoading = false;
        }
    }
}
