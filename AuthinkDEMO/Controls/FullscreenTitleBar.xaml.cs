using Windows.ApplicationModel.Core;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace AuthinkDEMO.Controls
{
    public sealed partial class FullscreenTitleBar : UserControl
    {
        double d = 0.0;

        public FullscreenTitleBar()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += Window_SizeChanged;
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
        }

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.Escape && ApplicationView.GetForCurrentView().IsFullScreenMode)
            {
                ApplicationView.GetForCurrentView().ExitFullScreenMode();
            }
        }

        private void Window_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            if (ApplicationView.GetForCurrentView().IsFullScreenMode)
            {
                CustomTitleBar.Visibility = Visibility.Collapsed;
            }
            else
            {
                CustomTitleBar.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
        }

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {
            CoreApplicationViewTitleBar titleBar = CoreApplication.GetCurrentView().TitleBar;
            titleBar.ExtendViewIntoTitleBar = true;
            titleBar.LayoutMetricsChanged += TitleBar_LayoutMetricsChanged;
        }

        private void TitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            GrabPanel.Height = sender.Height;
            FullScreenButton.Margin = new Thickness(0, 0, sender.SystemOverlayRightInset, 0);
            Window.Current.SetTitleBar(GrabPanel);
        }
    }
}
