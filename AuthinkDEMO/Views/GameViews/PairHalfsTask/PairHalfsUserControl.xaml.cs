using System;
using AuthinkDEMO.Services;
using AuthinkDEMO.ViewModel.GameViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AuthinkDEMO.Views.GameViews.PairHalfsTask
{
    public sealed partial class PairHalfsUserControl : UserControl
    {


        public int TaskCounter { get; set; }
        private PairHalfsViewModel vmodel { get; set; }
        private NavigationService navigationService { get; set; }

        public PairHalfsUserControl()
        {
            this.InitializeComponent();
            vmodel = (PairHalfsViewModel)this.DataContext;
            TaskCounter = 0;
            navigationService = new NavigationService();
        }

        private async void DropSuccessfull(object sender, EventArgs e)
        {
            TaskCounter++;
            if (vmodel.PictureCount == TaskCounter)
            {
                TaskCounter = 0;
                await System.Threading.Tasks.Task.Delay(2000);
                navigationService.NavigateTo(typeof (RewardView));
            }
        }
    }
}
