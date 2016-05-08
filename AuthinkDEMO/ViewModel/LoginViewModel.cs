using AuthinkDEMO.Views;
using AuthinkDEMO.Views.ChildlrenListView;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using AuthinkDEMO.Model.Data;
using AuthinkDEMO.Services;
using Windows.Storage;

namespace AuthinkDEMO.ViewModel
{
    public partial class LoginViewModel:ViewModelBase
    {
        public RelayCommand LoginCommand { get; set; }
        public async void Login()
        {
            if(!InternetConnection.IsAvailable())
            {
                this.ErrorMessage = "Internet connection is not available :(";
            }
            else
            {
                var isServerAvailble = await InternetConnection.IsServer();
                if(!isServerAvailble)
                {
                    this.ErrorMessage = "Service is not availble";
                }
                else if ((!string.IsNullOrEmpty(this.Username) && !string.IsNullOrEmpty(this.Password)) && loginService.Login(this.Username, this.Password))
                {
                    ApplicationData.Current.LocalSettings.Values["Username"] = this.Username;
                    navigationService.NavigateTo(typeof(ChildrenView));
                }
                else
                {
                    this.ErrorMessage = "User not found";
                }
            }
        }
    }

    public partial class LoginViewModel
    {
        public LoginViewModel
        (
            ILoginService     loginService,
            NavigationService navigationService
        )
        {
            this.loginService      = loginService;
            this.navigationService = navigationService;

            this.LoginCommand     = new RelayCommand(Login);
            
        }

        private readonly ILoginService     loginService;
        private readonly NavigationService navigationService;

        public string Username
        {
            get { return _username; }
            set
            {
                if (_username == value)
                {
                    return;
                }

                _username = value;
                this.RaisePropertyChanged(this.Username);
            }
        }

        private string _username;

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password == value)
                {
                    return;
                }

                _password = value;
                this.RaisePropertyChanged(this.Password);
            }
        }
        private string _password;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                if (_errorMessage == value)
                {
                    return;
                }

                _errorMessage = value;
                this.RaisePropertyChanged("ErrorMessage");
            }
        }
        private string _errorMessage;
    }
}
