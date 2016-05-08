using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AuthinkDEMO.Views.ChildlrenListView
{
    public sealed partial class ChildrenItemsControl : UserControl
    {
        public ChildrenItemsControl()
        {
            this.InitializeComponent();
        }

        private void HoverStarted(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            //var hoverItem = sender as Grid;
            //var hoverEllipse = (Ellipse) hoverItem.Children.First(child => child is Ellipse);
            //var hoverFirstName =
            //    (TextBlock)hoverItem.Children.First(Child => Child is TextBlock && (Child as TextBlock).Name == "FirstName");
            //var hoverLastName =
            //   (TextBlock) hoverItem.Children.First(Child => Child is TextBlock && (Child as TextBlock).Name == "LastName");
            this.HoverStoryboard.Begin();
        }

        private void HoverEnded(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            this.HoverStoryboard.Stop();
        }
    }
}
