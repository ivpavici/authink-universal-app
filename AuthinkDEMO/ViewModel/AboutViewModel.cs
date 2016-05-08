using AuthinkDEMO.Services;
using AuthinkDEMO.Settings.Language;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace AuthinkDEMO.ViewModel
{
    public class AboutViewModel : ViewModelBase
    {
        public string AboutTextContent
        {
            get { return _aboutTextContent; }
            set
            {
                if (_aboutTextContent == value)
                {
                    return;
                }

                _aboutTextContent = value;
                this.RaisePropertyChanged("AboutTextContent");
            }
        }
        private string _aboutTextContent = Language.AboutPage.AboutTextContent();

        public string BackButtonContent
        {
            get { return _backButtonContent; }
            set
            {
                if (_backButtonContent == value)
                {
                    return;
                }

                _backButtonContent = value;
                this.RaisePropertyChanged("BackButtonContent");
            }
        }
        private string _backButtonContent = Language.AboutPage.BackButtonContent();

        public RelayCommand BackButtonCommand { get; private set; }
        private void Back()
        {
            _navigationService.NavigateTo(typeof(MainPage));
        }
        private readonly NavigationService _navigationService;
        public AboutViewModel
            (
                NavigationService navigationService
            )
        {
            this._navigationService = navigationService;
            this.BackButtonCommand = new RelayCommand(Back);
        }
    }
}