using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Windows.UI.Xaml;

using AuthinkDEMO.Model.Queries;
using AuthinkDEMO.Services;
using AuthinkDEMO.Views;
using AuthinkDEMO.Settings.Language;

namespace AuthinkDEMO.ViewModel
{
    public partial class EndTestViewModel : ViewModelBase
    {
        public string SuccessfullTextContent
        {
            get { return _successfullTextContent; }
            set
            {
                if (_successfullTextContent == value)
                {
                    return;
                }

                _successfullTextContent = value;
                this.RaisePropertyChanged(this.SuccessfullTextContent);
            }
        }
        private string _successfullTextContent = Language.EndTestPage.SuccessfullTextContent();

        public string ResetTestButtonContent
        {
            get { return _resetTestButtonContent; }
            set
            {
                if (_resetTestButtonContent == value)
                {
                    return;
                }

                _resetTestButtonContent = value;
                this.RaisePropertyChanged(this.ResetTestButtonContent);
            }
        }
        private string _resetTestButtonContent = Language.EndTestPage.ResetTestButtonContent();

        public string TestMenuButtonContent
        {
            get { return _testMenuButtonContent; }
            set
            {
                if (_testMenuButtonContent == value)
                {
                    return;
                }

                _testMenuButtonContent = value;
                this.RaisePropertyChanged(this.TestMenuButtonContent);
            }
        }
        private string _testMenuButtonContent = Language.EndTestPage.TestMenuButtonContent();

        public string ExitButtonContent
        {
            get { return _exitButtonContent; }
            set
            {
                if (_exitButtonContent == value)
                {
                    return;
                }

                _exitButtonContent = value;
                this.RaisePropertyChanged(this.ExitButtonContent);
            }
        }
        private string _exitButtonContent = Language.EndTestPage.ExitButtonContent();
       
        public RelayCommand ResetTestCommand { get; set; }
        public void ResetTest()
        {
            var taskIds = taskQueries.GetAllTasksForTest(GameState.CurrentTestId).Select(task=>task.Id).ToList();
            GameState.Start(GameState.CurrentTestId, taskIds);
            navigationService.NavigateTo(typeof (GameView));
        }

        public RelayCommand GoToTestListCommand { get; set; }
        public void GoToTestList()
        {
            navigationService.NavigateTo(typeof (TestListView));
        }

        public RelayCommand ExitToMainCommand { get; set; }
        public void ExitToMain()
        {
            navigationService.NavigateTo(typeof(MainPage));
        }

    }

    public partial class EndTestViewModel
    {
        public EndTestViewModel
        (
            NavigationService navigationService,
            ITaskQueries      taskQueries
        )
        {
            this.ResetTestCommand    = new RelayCommand(ResetTest   );
            this.GoToTestListCommand = new RelayCommand(GoToTestList);
            this.ExitToMainCommand = new RelayCommand(ExitToMain);

            this.navigationService = navigationService;
            this.taskQueries        = taskQueries;
        }

        private readonly NavigationService navigationService;
        private readonly ITaskQueries      taskQueries;
    }
}