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
    public class CustomerDetailViewModel : AsyncViewModelBase
    {
        private IDataService _dataService;
        private INavigationService _navigationService;
        private IDialogService _dialogService;
        private IResourceService _resourceService;
        private ICameraService _cameraService;

        public RelayCommand EditCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand<Customer> InitCommand { get; private set; }
        public RelayCommand CapturePhotoCommand { get; private set; }

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

        public CustomerDetailViewModel(IDataService dataService, INavigationService navigationService, IDialogService dialogService, IResourceService resourceService, ICameraService cameraService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _dataService = dataService;
            _resourceService = resourceService;
            _cameraService = cameraService;

            InitCommand = new RelayCommand<Customer>(Init);
            EditCommand = new RelayCommand(Edit);
            DeleteCommand = new RelayCommand(Delete);
            CapturePhotoCommand = new RelayCommand(CapturePhoto);

            Messenger.Default.Register<NotificationMessage<Customer>>(this, NotificationReceived);
        }

        private void Init(Customer customer)
        {
            Customer = customer;
        }

        private void Edit()
        {
            _navigationService.NavigateTo(ViewModelLocator.CustomerEditPageKey, _customer);
        }

        private void Delete()
        {
            _dialogService.ShowMessage(String.Format(_resourceService.GetString("CustomerDetailConfirmDeleteMessage"), _customer.FirstName, _customer.LastName),
                                        _resourceService.GetString("CustomerDetailConfirmDeleteTitle"),
                                        _resourceService.GetString("CustomerDetailConfirmDeleteButtonOK"),
                                        _resourceService.GetString("CustomerDetailConfirmDeleteButtonCancel"), 
                                        async (confirmed) =>
            {
                if (confirmed)
                {
                    var result = await _dataService.DeleteCustomerAsync(_customer.Id);
                    if (result.IsSuccessStatusCode)
                    {
                        Messenger.Default.Send<NotificationMessage<Customer>>(new NotificationMessage<Customer>(null, ViewModelLocator.CustomersReloadNotification));
                        _navigationService.GoBack();
                    }
                    else
                    {
                        await _dialogService.ShowError(_resourceService.GetString("CustomersDetailErrorDeleteMessage"), _resourceService.GetString("CustomersDetailErrorDeleteTitle"), _resourceService.GetString("CustomersDetailErrorDeleteButton"), null);
                    }
                }
            });
        }

        private void NotificationReceived(NotificationMessage<Customer> message)
        {
            if (message.Notification == ViewModelLocator.CustomersReloadNotification && message.Content != null)
                Customer = message.Content;
        }

        private async void CapturePhoto()
        {
            var photo = await _cameraService.CapturePhotoAsync();
            
        }
    }
}
