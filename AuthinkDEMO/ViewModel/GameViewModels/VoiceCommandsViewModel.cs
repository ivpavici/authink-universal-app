using System;
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
using System.Collections.Generic;

namespace AuthinkDEMO.ViewModel.GameViewModels
{
    public partial class VoiceCommandsViewModel : ViewModelBase
    {
        private async void ItemClicked(ItemClickEventArgs e)
        {
            var picture = (ent::Picture.AnswerPicture)e.ClickedItem;
            var source = (ListView) e.OriginalSource;
            if(picture.IsAnswer)
            {
                source.IsHitTestVisible = false;
                SoundServices.Instance.Play();

                await System.Threading.Tasks.Task.Delay(2000);

                navigationService.NavigateTo(typeof (RewardView));
            }
            
        }
        private void TransformPicturesDataToModelData(List<ent::Picture.AnswerPicture> picturesData)
        {
            picturesData.Shuffle();

            foreach (var picture in picturesData)
            {
                Pictures.Add(picture);
            }

            var correctPicture = picturesData.Where(picture => picture.IsAnswer).SingleOrDefault();

            for (var i = 0; i < Pictures.Count; i++)
            {
                if (Pictures[i].Id == correctPicture.Id)
                {
                    Pictures[i].IsAnswer = true;
                }
            }
        }

        private void Init()
        {
            var pictures = pictureQueries.GetAllPicturesForTask(GameState.GetTask())
                                          .Select(picture => (ent::Picture.AnswerPicture)picture)
                                          .ToList();

            SoundUrl = SoundServices.GetInstructionsSoundUrl
            (
                sound: taskQueries.GetSingle_byId(GameState.GetTask()).VoiceCommand
            );

            TransformPicturesDataToModelData
            (
                picturesData: pictures
            );
        }
        /// <summary>
        /// HAX
        /// </summary>
        public RelayCommand<object> ReplayVoiceInstructionCommand { get; set; }
        public void ReplayVoiceInstruction(object element)
        {
            var pave = element as MediaElement;
            pave.Position = TimeSpan.Zero;
            pave.Play();
        }
        //TODO:REFACTORAT
    }

    public partial class VoiceCommandsViewModel
    {
        public VoiceCommandsViewModel
        (
            IPictureQueries   pictureQueries,
            ITaskQueries      taskQueries,
            NavigationService navigationService
        )
        {
            this.pictureQueries    = pictureQueries;
            this.taskQueries       = taskQueries;
            this.navigationService = navigationService;

            this.ItemClickCommand = new RelayCommand<ItemClickEventArgs>(ItemClicked);
            this.Pictures         = new ObservableCollection<ent::Picture.AnswerPicture>();

            this.ReplayVoiceInstructionCommand = new RelayCommand<object>(ReplayVoiceInstruction);

            Init();
        }

        private readonly IPictureQueries pictureQueries;
        private readonly ITaskQueries    taskQueries;
        private readonly NavigationService navigationService;

        public ObservableCollection<ent::Picture.AnswerPicture> Pictures { get; set; }
        public Uri SoundUrl { get; set; }

        public RelayCommand<ItemClickEventArgs> ItemClickCommand { get; set; }
        public RelayCommand OnNavigatedToCommand { get; set; }
    }
}