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

using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace AuthinkDEMO.ViewModel.GameViewModels
{
    public partial class DetectDifferentItemsUserViewModel : ViewModelBase
    {
        private async void ItemClick(ItemClickEventArgs e)
        {
            var lista           = (ListView)e.OriginalSource;
            var selectedPicture = (DetectDifferentPicture)e.ClickedItem;
            var kontejner       = lista.Parent as StackPanel;
            var slika           = kontejner.Children.FirstOrDefault(x => x is Image) as Image;

            if (selectedPicture.IsAnswer)
            {
                _counter++;
                SoundServices.Instance.Play();
                slika.Source = new BitmapImage(new Uri("ms-appx:///Resources/Nagradni ekran/checkmark.png", UriKind.RelativeOrAbsolute));
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

        private List<DetectDifferentPicture> GeneratePicturesRow(List<ent::Picture.AnswerPicture> picturesData)
        {
            var correctPicture = workingCopy.Select(picture => new DetectDifferentPicture(picture.Url, picture.Id, true))
                                            .First();

            workingCopy.Remove(workingCopy.First());

            if (!workingCopy.Any())
            {
                workingCopy = pictures.Select(picture => picture).ToList();
            }

            var result = new List<DetectDifferentPicture>();

            for (var i = 0; i < pictures.Count; i++)
            {
                result.Add(workingCopy.Select(picture => new DetectDifferentPicture(picture.Url, picture.Id, false)).First());
            }

            result.Add(correctPicture);
            result.Shuffle();

            return result;
        }

        private void TransformPicturesDataToModelData()
        {
            this.workingCopy = new List<ent::Picture.AnswerPicture>(pictures);

            this.Pictures_first_list  = new ObservableCollection<DetectDifferentPicture>(GeneratePicturesRow(pictures));
            this.Pictures_second_list = new ObservableCollection<DetectDifferentPicture>(GeneratePicturesRow(pictures));
            this.Pictures_third_list  = new ObservableCollection<DetectDifferentPicture>(GeneratePicturesRow(pictures));
            this.Pictures_fourth_list = new ObservableCollection<DetectDifferentPicture>(GeneratePicturesRow(pictures));
        }
        private void Init()
        {
            this.pictures = pictureQueries.GetAllPicturesForTask(GameState.GetTask())
                                          .Select(picture => (ent::Picture.AnswerPicture)picture)
                                          .ToList();

            SoundUrl = SoundServices.GetInstructionsSoundUrl
            (
                sound: taskQueries.GetSingle_byId(GameState.GetTask()).VoiceCommand
            );

            TransformPicturesDataToModelData();
        }
    }

    public partial class DetectDifferentItemsUserViewModel
    {
        public DetectDifferentItemsUserViewModel
        (
            IPictureQueries   pictureQueries,
            ITaskQueries      taskQueries,
            NavigationService navigationService
        )
        {
            this.taskQueries       = taskQueries;
            this.pictureQueries    = pictureQueries;
            this.navigationService = navigationService;

            ItemClickCommand = new RelayCommand<ItemClickEventArgs>(ItemClick);

            Init();
        }

        private readonly ITaskQueries      taskQueries;
        private readonly IPictureQueries   pictureQueries;
        private readonly NavigationService navigationService;
        public Uri SoundUrl { get; set; }

        private List<ent::Picture.AnswerPicture> workingCopy { get; set; }
        private List<ent::Picture.AnswerPicture> pictures { get; set; }

        private static int _counter = 0;

        public DetectDifferentPicture Correct_first_list { get; set; }
        public DetectDifferentPicture Correct_second_list { get; set; }
        public DetectDifferentPicture Correct_third_list { get; set; }
        public DetectDifferentPicture Correct_fourth_list { get; set; }
        public ObservableCollection<DetectDifferentPicture> Pictures_first_list { get; set; }
        public ObservableCollection<DetectDifferentPicture> Pictures_second_list { get; set; }
        public ObservableCollection<DetectDifferentPicture> Pictures_third_list { get; set; }
        public ObservableCollection<DetectDifferentPicture> Pictures_fourth_list { get; set; }

        public RelayCommand<ItemClickEventArgs> ItemClickCommand { get; set; }

    }

    public class DetectDifferentPicture
    {
        public DetectDifferentPicture
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