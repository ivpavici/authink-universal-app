using System;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using System.Linq;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight.Command;
using Windows.UI.Xaml.Input;
using System.Collections.Generic;

using ent = AuthinkDEMO.Model.Entities;
using AuthinkDEMO.Views;
using AuthinkDEMO.Model.Queries;
using AuthinkDEMO.Services;

namespace AuthinkDEMO.ViewModel.GameViewModels
{
    public partial class DetectDifferentItems_HighDifficulty_ViewModel
    {
        private async void PointerPressed(PointerRoutedEventArgs e)
        {
            var img = (Image) e.OriginalSource;

            if (this.CorrectPicture.Id == Convert.ToInt32(img.Tag))
            {
                img.IsHitTestVisible = false;

                SoundServices.Instance.Play();

                await System.Threading.Tasks.Task.Delay(2000);

                navigationService.NavigateTo(typeof(RewardView));
            }
        }

        public void TransformPicturesDataToModelData(List<ent::Picture.AnswerPicture> picturesData)
        {
            var picture_correctAnswer = picturesData.FirstOrDefault(picture => picture.IsAnswer);
            var picture_wrongAnswer   = picturesData.FirstOrDefault(picture => !picture.IsAnswer);

            for (var i = 0; i < 7; i++)
            {
                Pictures.Add(picture_wrongAnswer);
            }

            Pictures.Add(picture_correctAnswer);

            this.CorrectPicture = picture_correctAnswer;

            this.Pictures.Shuffle();
        }
        private void Init()
        {
            var picturesData = pictureQueries.GetAllPicturesForTask(GameState.GetTask())
                                             .Select(picture => (ent::Picture.AnswerPicture)picture)
                                             .ToList();

            SoundUrl = SoundServices.GetInstructionsSoundUrl
            (
                sound: taskQueries.GetSingle_byId(GameState.GetTask()).VoiceCommand
            );

            TransformPicturesDataToModelData(picturesData);
        }
    }
    public partial class DetectDifferentItems_HighDifficulty_ViewModel
    {
        public DetectDifferentItems_HighDifficulty_ViewModel
        (
            IPictureQueries   pictureQueries,
            ITaskQueries      taskQueries,
            NavigationService navigationService
        )
        {
            this.pictureQueries    = pictureQueries;
            this.taskQueries       = taskQueries;
            this.navigationService = navigationService;

            this.Pictures              = new ObservableCollection<ent::Picture.AnswerPicture>();
            this.PointerPressedCommand = new RelayCommand<PointerRoutedEventArgs>(PointerPressed);

            Init();
        }

        private readonly IPictureQueries   pictureQueries;
        private readonly ITaskQueries      taskQueries;
        private readonly NavigationService navigationService;

        public Uri SoundUrl { get; set; }

        public ObservableCollection<ent::Picture.AnswerPicture> Pictures { get; set; }
        public ent::Picture.AnswerPicture                       CorrectPicture { get; set; }

        public RelayCommand<PointerRoutedEventArgs> PointerPressedCommand { get; set; }
    }
}
