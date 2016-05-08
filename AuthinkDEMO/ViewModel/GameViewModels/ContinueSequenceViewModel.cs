using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

using ent = AuthinkDEMO.Model.Entities;
using AuthinkDEMO.Model.Queries;
using AuthinkDEMO.Services;
using AuthinkDEMO.Views;
using System.Collections.Generic;

namespace AuthinkDEMO.ViewModel.GameViewModels
{
    public partial class ContinueSequenceViewModel : ViewModelBase
    {
        private async void ItemClicked(ItemClickEventArgs e)
        {
            var picture = (ent::Picture.AnswerPicture)e.ClickedItem;
            var source  = (ListView)e.OriginalSource;

            if (picture.IsAnswer)
            {
                var pictureToUpdate = Pictures_Sequence.Last();
                pictureToUpdate.Url = picture.Url;

                Pictures_Sequence[Pictures_Sequence.IndexOf(pictureToUpdate)] = pictureToUpdate;

                SoundServices.Instance.Play();
                
                source.IsHitTestVisible = false;

                await System.Threading.Tasks.Task.Delay(2000);

                navigationService.NavigateTo(typeof(RewardView));
            }
        }
        
        public void TransformPicturesDataToModelData(List<ent::Picture.AnswerPicture> picturesData)
        {
            picturesData.Shuffle();

            var randomIndex     = Random.Next(1, picturesData.Count);
            this.CorrectPicture = picturesData[randomIndex];

            foreach (var picture in picturesData)
            {
                Pictures_OfferedAnswers.Add(picture);
            }

            for (var i = 0; i < randomIndex; i++)
            {
                picturesData.Add(picturesData[i]);
            }
            
            foreach (var picture in picturesData)
            {
                Pictures_Sequence.Add(picture);
            }

            Pictures_Sequence.Add(new ent::Picture.AnswerPicture());

            this.CorrectPicture.IsAnswer = true;

            for (var i = 0; i < Pictures_OfferedAnswers.Count; i++)
            {
                if (Pictures_OfferedAnswers[i].Id == this.CorrectPicture.Id)
                {
                    Pictures_OfferedAnswers[i].IsAnswer = true;
                }
            }

            this.Pictures_OfferedAnswers.Shuffle();
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

    public partial class ContinueSequenceViewModel
    {
        public ContinueSequenceViewModel
        (
            IPictureQueries   pictureQueries,
            ITaskQueries      taskQueries,
            NavigationService navigationService,
            Random            random
        )
        {
            this.pictureQueries    = pictureQueries;
            this.taskQueries       = taskQueries;
            this.navigationService = navigationService;

            this.Pictures_OfferedAnswers     = new ObservableCollection<ent::Picture.AnswerPicture>();
            this.Pictures_Sequence = new ObservableCollection<ent::Picture.AnswerPicture>();

            this.ItemClickCommand = new RelayCommand<ItemClickEventArgs>(ItemClicked);

            this.Random = random;

            Init();
        }

        private readonly IPictureQueries   pictureQueries;
        private readonly ITaskQueries      taskQueries;
        private readonly NavigationService navigationService;

        public ent::Picture.AnswerPicture CorrectPicture { get; set; }
        public Uri SoundUrl { get; set; }

        public ObservableCollection<ent::Picture.AnswerPicture> Pictures_OfferedAnswers { get; set; }
        public ObservableCollection<ent::Picture.AnswerPicture> Pictures_Sequence { get; set; }

        public RelayCommand<ItemClickEventArgs> ItemClickCommand { get; set; }

        private Random Random { get; set; }
    }
}