using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using AuthinkDEMO.Model.Queries;
using AuthinkDEMO.Services;
using AuthinkDEMO.Views;
using ent = AuthinkDEMO.Model.Entities;
using AuthinkDEMO.Settings.Language;

namespace AuthinkDEMO.ViewModel
{
    public partial class TestListViewModel
    {
        public ent::Test SelectedTest
        {
            get { return _selectedTest; }
            set
            {
                if (_selectedTest == value)
                {
                    return;
                }

                _selectedTest = value;
                RaisePropertyChanged("SelectedTest");
            }

        }

        private ent::Test _selectedTest;

        public ent::Task SelectedTask
        {
            get { return _selectedTask; }
            set
            {
                if (_selectedTask == value)
                {
                    return;
                }

                _selectedTask = value;
                RaisePropertyChanged("SelectedTask");
            }

        }

        private ent::Task _selectedTask;

        public string SelectTestButtonContent
        {
            get { return _selectTestButtonContent; }
            set
            {
                if (_selectTestButtonContent == value)
                {
                    return;
                }

                _selectTestButtonContent = value;
                this.RaisePropertyChanged(this.SelectTestButtonContent);
            }
        }
        public string GoBackButtonContent
        {
            get { return _goBackButtonContent; }
            set
            {
                if (_goBackButtonContent == value)
                {
                    return;
                }

                _goBackButtonContent = value;
                this.RaisePropertyChanged(this.GoBackButtonContent);
            }
        }

        private string _selectTestButtonContent = Language.TestListPage.SelectTestButtonContent();
        private string _goBackButtonContent = Language.TestListPage.GoBackButtonContent();

        public RelayCommand RunTestCommand { get; private set; }
        private void RunTest()
        {
            var tasks_ids = taskQueries.GetAllTasksForTest(SelectedTest.Id).Select(task => task.Id).SkipWhile(x => SelectedTask != null && x != SelectedTask.Id).ToList();
            
            GameState.Start(SelectedTest.Id, tasks_ids);
            _navigationService.NavigateTo(typeof(GameView));
        }

        public RelayCommand GoBackCommand { get; private set; }
        private void GoBack()
        {
            _navigationService.NavigateTo(typeof(MainPage));
        }
    }
    public partial class TestListViewModel : ViewModelBase
    {
        private readonly NavigationService _navigationService;

        public TestListViewModel(
              NavigationService navigationService,
            ITestQueries testQueries,
            ITaskQueries taskQueries)
        {
            _navigationService = navigationService;
            
            this.taskQueries = taskQueries;
            Tests = new List<ent::Test>(testQueries.GetAll());

            this.RunTestCommand = new RelayCommand(RunTest);
            this.GoBackCommand = new RelayCommand(GoBack);
        }

        private readonly ITaskQueries taskQueries;

        public IReadOnlyList<ent::Test> Tests { get; set; }


    }
    
}