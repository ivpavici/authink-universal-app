using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AuthinkDEMO.Services;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace AuthinkDEMO.Views.GameViews.PairHalfsTask
{
    public sealed partial class PairHalfsTemplateUserControl : UserControl
    {
        public PairHalfsTemplateUserControl()
        {
            this.InitializeComponent();
        }
        public delegate void DropNavigate(object sender, EventArgs e);
        public event DropNavigate OnTemplateDropSuccessfull;

        private void _OnTemplateDropSuccessfull()
        {
            if (this.OnTemplateDropSuccessfull != null)
            {
                this.OnTemplateDropSuccessfull.Invoke(this, EventArgs.Empty);
            }
        }
        
        private void DropSuccessful(object sender, EventArgs e)
        {
            this.ShowWholePicture.Begin();
            _OnTemplateDropSuccessfull();


        }
    }
}
