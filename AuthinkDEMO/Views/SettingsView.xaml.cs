using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace AuthinkDEMO.Views
{
    public sealed partial class SettingsView
    {
        public SettingsView()
        {
            this.InitializeComponent();
        }

        private void ChooselanguageButton_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var radioButton = (RadioButton)sender;

            if (ApplicationData.Current.LocalSettings.Values["Language"].ToString() == "Hr" && radioButton.Name == "ChooseCroatianButton")
            {
                radioButton.IsChecked = true;
            }
            else if (ApplicationData.Current.LocalSettings.Values["Language"].ToString() == "En" && radioButton.Name == "ChooseEnglishButton")
            {
                radioButton.IsChecked = true;
            }
        }

        private void RadioButton_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var radioButton = (RadioButton)sender;

            if ((bool)ApplicationData.Current.LocalSettings.Values["IsRewardSoundEnabled"] && radioButton.Name == "EnableRewardSound")
            {
                radioButton.IsChecked = true;
            }
            else if (!(bool)ApplicationData.Current.LocalSettings.Values["IsRewardSoundEnabled"] && radioButton.Name == "DisableRewardSound")
            {
                radioButton.IsChecked = true;
            }

            if ((bool)ApplicationData.Current.LocalSettings.Values["IsInstructionSoundEnabled"] && radioButton.Name == "EnableSound_instructions")
            {
                radioButton.IsChecked = true;
            }
            else if (!(bool)ApplicationData.Current.LocalSettings.Values["IsInstructionSoundEnabled"] && radioButton.Name == "DisableSound_instructions")
            {
                radioButton.IsChecked = true;
            }
        }
    }
}
