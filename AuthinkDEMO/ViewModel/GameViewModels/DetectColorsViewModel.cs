using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using GalaSoft.MvvmLight.Command;

using ent = AuthinkDEMO.Model.Entities;
using AuthinkDEMO.Model;
using AuthinkDEMO.Model.Data.Private;
using AuthinkDEMO.Model.Queries;
using AuthinkDEMO.Services;
using AuthinkDEMO.Views;

namespace AuthinkDEMO.ViewModel.GameViewModels
{
    public partial class DetectColorsViewModel: ViewModelBase
    {
        public DetectColorsViewModel
        (
            ITaskQueries      taskQueries,
            IPictureQueries   pictureQueries
        )
        {
            this.taskQueries    = taskQueries;
            this.pictureQueries = pictureQueries;

            Init();
        }

        private readonly ITaskQueries    taskQueries;
        private readonly IPictureQueries pictureQueries;
        public Uri SoundUrl { get; set; }

        public ObservableCollection<DetectColorPicture> PicturesWithColors { get; set; }
    }
    public partial class DetectColorsViewModel
    {
        private void Init()
        {
            var picturesData = pictureQueries.GetAllPicturesForTask(GameState.GetTask())
                                              .Select(picture => (ent::Picture.ColorPicture)picture)
                                              .ToList();

            SoundUrl = SoundServices.GetInstructionsSoundUrl
            (
                sound: taskQueries.GetSingle_byId(GameState.GetTask()).VoiceCommand
            );

            TransformPicturesDataToModelData(picturesData);
        }
        private void TransformPicturesDataToModelData(List<ent::Picture.ColorPicture> picturesData)
        {
            PicturesWithColors = new ObservableCollection<DetectColorPicture>();

            var transformedPicturesData = picturesData.Select(picture => new DetectColorPicture
            (
                url: picture.Url,
                id:  picture.Id,

                colors:        picture.Colors,
                picturesCount: picturesData.Count()
            )).ToList();

            foreach(var picture in transformedPicturesData)
            {
                this.PicturesWithColors.Add(picture);
            }
        }
    }
    public class DetectColorPicture
    {
        public DetectColorPicture
        (
            string url,
            int    id,

            List<ent::Color> colors,
            int              picturesCount
        )
        {
            this.Url = url;
            this.Id   = id;

            this.Colors         = colors;
            this.picturesCount = picturesCount;

            this.EllipseButtonClickCommand = new RelayCommand<object>(EllipseButtonClick);
            this._navigationService        = new NavigationService();

            this.Colors.Shuffle();
        }
        public string Url { get; set; }
        public int    Id { get; set; }
        public List<ent::Color> Colors { get; set; }
        public RelayCommand<object> EllipseButtonClickCommand { get; private set; }
        
        private static int _counter = 0;
        private int        picturesCount;
        private readonly NavigationService _navigationService;

        private async void EllipseButtonClick(object e)
        {
            var button    = e as Button;
            var container = button.Parent as Grid;
            var buttons   = container.Children.Where(child => child is Button);
            var pictures  = container.Children.OfType<Image>().ToList();

            var checkmark = pictures.SingleOrDefault(x => (string)x.Tag == "Checkmark");

            if (button.Tag.ToString() == "True")
            {
                checkmark.Source = new BitmapImage(new Uri("ms-appx:///Resources/Nagradni ekran/checkmark.png", UriKind.RelativeOrAbsolute));
                SoundServices.Instance.Play();

                _counter++;
                foreach (var x in buttons)
                {
                    x.IsHitTestVisible = false;
                }
            }
            else
            {
                checkmark.Source = new BitmapImage(new Uri("ms-appx:///Resources/Nagradni ekran/crossmark.png", UriKind.RelativeOrAbsolute));

            }

            if (picturesCount == _counter)
            {
                _counter = 0;

                await System.Threading.Tasks.Task.Delay(2000);

                _navigationService.NavigateTo(typeof(RewardView));
            }
        }
    }
}