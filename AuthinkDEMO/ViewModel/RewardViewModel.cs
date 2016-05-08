using System;
using System.Linq;
using AuthinkDEMO.Services;
using AuthinkDEMO.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Windows.Storage;
using AuthinkDEMO.Model;
using AuthinkDEMO.Settings.Language;

namespace AuthinkDEMO.ViewModel
{
    public partial class RewardViewModel : ViewModelBase
    {
        public RelayCommand TempRewordCommand { get; private set; }
        public void Continue()
        {
            GameState.EndTask();
            //System.Threading.Tasks.Task.Delay(2000);
            if(GameState.TaskIds.Any())
            {
                
                _navigationService.NavigateTo(typeof(GameView));
            }
                
            else
            {
                _navigationService.NavigateTo(typeof(EndTestView));
            } 
        }

        public string ContinueButtonContent
        {
            get { return _continueButtonContent; }
            set
            {
                if (_continueButtonContent == value)
                {
                    return;
                }

                _continueButtonContent = value;
                this.RaisePropertyChanged(this.ContinueButtonContent);
            }
        }
        private string _continueButtonContent = Language.RewardPage.ContinueButtonContent();

        public string RewardTextContent
        {
            get { return _rewardTextContent; }
            set
            {
                if (_rewardTextContent == value)
                {
                    return;
                }

                _rewardTextContent = value;
                this.RaisePropertyChanged(this.RewardTextContent);
            }
        }
        private string _rewardTextContent = Language.RewardPage.RewardTextContent();
    }

    public partial class RewardViewModel
    {
        private readonly NavigationService _navigationService;
        public Uri SoundUrl { get; set; }

        public RewardViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;
            this.TempRewordCommand = new RelayCommand(Continue);
            this.SoundUrl = (bool)ApplicationData.Current.LocalSettings.Values["IsRewardSoundEnabled"] ? new Uri("ms-appx:///Resources/Sounds/aplauz-dugi.mp3") : null;
        }
    }
}