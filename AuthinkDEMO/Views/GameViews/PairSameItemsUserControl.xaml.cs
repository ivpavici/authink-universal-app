using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AuthinkDEMO.Services;
using AuthinkDEMO.ViewModel.GameViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AuthinkDEMO.Views.GameViews
{
    public sealed partial class PairSameItemsUserControl : UserControl
    {
        public int numberOfSuccessfulDrops { get; set; }
        private PairSameItemsViewModel vmodel;
        private NavigationService navigationService;
        public PairSameItemsUserControl()
        {
            this.InitializeComponent();
            vmodel = (PairSameItemsViewModel) this.DataContext;
            navigationService = new NavigationService();
        }

        private async void  DropSuccessful(object sender, EventArgs e)
        {
            numberOfSuccessfulDrops++;
            if (numberOfSuccessfulDrops == vmodel.ItemsSelectionList.Count)
            {
                await System.Threading.Tasks.Task.Delay(2000);
                navigationService.NavigateTo(typeof(RewardView));
            }
        }
    }
}
