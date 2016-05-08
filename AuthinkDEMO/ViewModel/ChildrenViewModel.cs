using System.Collections.ObjectModel;
using AuthinkDEMO.Settings.Language;
using ent = AuthinkDEMO.Model.Entities;
using AuthinkDEMO.Model.Data;
using AuthinkDEMO.Services;
using AuthinkDEMO.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace AuthinkDEMO.ViewModel
{
    public partial class ChildrenViewModel:ViewModelBase
    {
        public string FirstNameContent
        {
            get { return _firstNameContent; }
            set
            {
                if (_firstNameContent == value)
                {
                    return;
                }

                _firstNameContent = value;
                this.RaisePropertyChanged(this.FirstNameContent);
            }
        }

        private string _firstNameContent = Language.ChildrenPage.FirstNameContent();

        public string LastNameContent
        {
            get { return _lastNameContent; }
            set
            {
                if (_lastNameContent == value)
                {
                    return;
                }

                _lastNameContent = value;
                this.RaisePropertyChanged(this.LastNameContent);
            }
        }

        private string _lastNameContent = Language.ChildrenPage.LastNameContent();

        public RelayCommand<ItemClickEventArgs> SelectChildCommand { get; set; }
        public void SelectChild (ItemClickEventArgs e)
        {
            var selectedChild = e.ClickedItem as ent::Child;

            ApplicationData.Current.LocalSettings.Values["SelectedChildId"] = selectedChild.Id;
            navigationService.NavigateTo(typeof (MainPage));
        }
    }

    public partial class ChildrenViewModel
    {
        public ChildrenViewModel
        (
            IDataProvider     dataProvider,
            NavigationService navigationService
        )
        {
            this.dataProvider      = dataProvider;
            this.navigationService = navigationService;

            this.Children = new ObservableCollection<ent::Child>(dataProvider.GetAll_children());
            
            this.SelectChildCommand = new RelayCommand<ItemClickEventArgs>(SelectChild);
        }

        private readonly IDataProvider     dataProvider;
        private readonly NavigationService navigationService;

        public ObservableCollection<ent::Child> Children { get; set; }
    }
}
