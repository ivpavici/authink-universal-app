using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ent = AuthinkDEMO.Model.Entities;
using AuthinkDEMO.Model.Queries;
using AuthinkDEMO.Services;
using AuthinkDEMO.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml;
namespace AuthinkDEMO.ViewModel.GameViewModels
{
    public partial class DetectSameItemsViewModel : ViewModelBase
    {   
        public void LoadedUserControl(RoutedEventArgs e)
        {
            var majka = e.OriginalSource;
        }

        private async void ItemClick(ItemClickEventArgs e)
        {
            var lista           = (ListView)e.OriginalSource;
            var selectedPicture = (DetectSamePicture)e.ClickedItem;
            var kontejner       = lista.Parent as StackPanel;

            BravoTalker= (kontejner.Parent as Grid).Children.First(child => (child is MediaElement)) as MediaElement;

            var slika = kontejner.Children.FirstOrDefault(x => x is Image) as Image;

            if (selectedPicture.IsAnswer)
            {
                _counter++;
                slika.Source       = new BitmapImage(new Uri("ms-appx:///Resources/Nagradni ekran/checkmark.png", UriKind.RelativeOrAbsolute));
                BravoTalker.Source = new Uri("ms-appx:///Resources/Sounds/bravo.mp3");
                BravoTalker.Play();

                lista.IsHitTestVisible = false;
            }
            else
            {
                slika.Source = new BitmapImage(new Uri("ms-appx:///Resources/Nagradni ekran/crossmark.png", UriKind.RelativeOrAbsolute));
             
            }
            if (_counter == 4)
            {
                _counter = 0;
                await System.Threading.Tasks.Task.Delay(2000);
                navigationService.NavigateTo(typeof(RewardView));
            }
        }
    }

    public partial class DetectSameItemsViewModel
    {
        public DetectSameItemsViewModel
        (
            IPictureQueries pictureQueries,
            NavigationService navigationService
        )
        {
           
            this.navigationService = navigationService;

            pictures = pictureQueries.GetAllPicturesForTask(GameState.GetTask())
                .Select(picture => (ent::Picture.AnswerPicture)picture)
                                         .ToList();
            workingCopy = new List<ent::Picture.AnswerPicture>(pictures);

            Pictures_first_list = new ObservableCollection<DetectSamePicture>(GetPictures());
            Pictures_second_list = new ObservableCollection<DetectSamePicture>(GetPictures());
            Pictures_third_list = new ObservableCollection<DetectSamePicture>(GetPictures());
            Pictures_fourth_list = new ObservableCollection<DetectSamePicture>(GetPictures());

            ItemClickCommand         = new RelayCommand<ItemClickEventArgs>(ItemClick);
            LoadedUserControlCommand = new RelayCommand<RoutedEventArgs>(LoadedUserControl);
        }

        public MediaElement BravoTalker { get; set; }

        private readonly NavigationService navigationService;

        public DetectSamePicture Correct_first_list { get; set; }
        public DetectSamePicture Correct_second_list { get; set; }
        public DetectSamePicture Correct_third_list { get; set; }
        public DetectSamePicture Correct_fourth_list { get; set; }

        private List<ent::Picture.AnswerPicture> workingCopy { get; set; }
        private List<ent::Picture.AnswerPicture> pictures { get; set; }

        private static int _counter = 0;

        public ObservableCollection<DetectSamePicture> Pictures_first_list { get; set; }
        public ObservableCollection<DetectSamePicture> Pictures_second_list { get; set; }
        public ObservableCollection<DetectSamePicture> Pictures_third_list { get; set; }
        public ObservableCollection<DetectSamePicture> Pictures_fourth_list { get; set; }

        public RelayCommand<ItemClickEventArgs> ItemClickCommand { get; set; }
        public RelayCommand<RoutedEventArgs> LoadedUserControlCommand { get; set; }

        public List<DetectSamePicture> GetPictures()
        {
            var correctPicture = workingCopy.Select(picture => new DetectSamePicture(picture.Url, picture.Id, true)).First();
            workingCopy.Remove(workingCopy.First());

            if (!workingCopy.Any())
            {
                workingCopy = pictures.Select(picture => picture).ToList();
            }

            var result = new List<DetectSamePicture>();
            foreach (var picture in pictures)
            {

                result.Add(new DetectSamePicture(picture.Url, picture.Id, false));
            }

            for (var i = 0; i < result.Count; i++)
            {
                if (result[i].Url == correctPicture.Url)
                {
                    result[i] = new DetectSamePicture(correctPicture.Url, correctPicture.Id, true);
                }
            }

            result.Add(correctPicture);
            result.Shuffle();

            return result;

        }
    }

    public class DetectSamePicture
    {
        public DetectSamePicture
        (
            string url,
            int id,
            bool isAnswer
        )
        {
            this.Url = url;
            this.Id = id;
            this.IsAnswer = isAnswer;
        }
        public string Url { get; set; }
        public int Id { get; set; }
        public bool IsAnswer { get; set; }
    }
}