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
using Windows.Storage.AccessCache;
using Windows.UI.Xaml.Controls;

namespace AuthinkDEMO.ViewModel.GameViewModels
{
    public partial class OrderBySizeViewModel : ViewModelBase
    {
        private  async void ItemClick(ItemClickEventArgs e)
        {
            var selectedPicture = (OrderBySizePicture) e.ClickedItem;

            if(selectedPicture.Index == CurrentIndex)
            {
                SoundServices.Instance.Play();

                this.SelectedPictures[CurrentIndex] = selectedPicture;
                this.Pictures.Remove(selectedPicture);

                CurrentIndex++;
            }

            if(!Pictures.Any())
            {
                await System.Threading.Tasks.Task.Delay(2000);

                navigationService.NavigateTo(typeof (RewardView));
            }
        }

        private void TransformPicturesDataToModelData(ent::Picture.AnswerPicture pictureData, int taskDifficulty)
        {
            var knownImageSizeMappings = new Dictionary<int, int>()
            {
                {0, 75}, {1, 100}, {2, 125}, {3, 150}, {4, 175},{5, 200},
            };

            for (var i = 0; i < taskDifficulty; i++)
            {
                Pictures.Add(new OrderBySizePicture(pictureData.Id, pictureData.Url, i, knownImageSizeMappings[i], knownImageSizeMappings[i]));
                this.SelectedPictures.Add(new OrderBySizePicture(0, "ms-appx:///Resources/placeholder.png", 0, 150, 150));
            }

            this.Pictures.Shuffle();
        }

        private void Init()
        {
            var task    = taskQueries.GetSingle_byId(GameState.GetTask()); ;
            var picture = pictureQueries.GetAllPicturesForTask(GameState.GetTask())
                                         .Select(x => (ent::Picture.AnswerPicture)x)
                                         .First();


            SoundUrl = SoundServices.GetInstructionsSoundUrl(task.VoiceCommand);

            TransformPicturesDataToModelData
            (
                pictureData:    picture,
                taskDifficulty: task.Difficulty
            );
        }
    }

    public partial class OrderBySizeViewModel 
    {
        public OrderBySizeViewModel
        (
            IPictureQueries   pictureQueries,
            ITaskQueries      taskQueries,
            NavigationService navigationService 
        )
        {
            this.pictureQueries    = pictureQueries;
            this.taskQueries       = taskQueries;
            this.navigationService = navigationService;

            this.CurrentIndex = 0;

            this.Pictures         = new ObservableCollection<OrderBySizePicture>();
            this.SelectedPictures = new ObservableCollection<OrderBySizePicture>();

            this.ItemClickCommand = new RelayCommand<ItemClickEventArgs>(ItemClick);

            Init();
        }

        private readonly IPictureQueries   pictureQueries;
        private readonly ITaskQueries      taskQueries;
        private readonly NavigationService navigationService;

        private int CurrentIndex { get; set; }

        public ObservableCollection<OrderBySizePicture> Pictures         { get; set; }
        public ObservableCollection<OrderBySizePicture> SelectedPictures { get; set; }

        public RelayCommand<ItemClickEventArgs> ItemClickCommand { get; set; }
        public Uri SoundUrl { get; set; }
    }

    public class OrderBySizePicture
    {
        public OrderBySizePicture
        (
            int    id,
            string url,
            int    index,
            int    height,
            int    width
         )
        {
            this.Id     = id;
            this.Url    = url;
            this.Index  = index;
            this.Height = height;
            this.Width  = width;
        }
        public int    Id     { get; private set; }
        public string Url    { get; private set; }
        public int    Index  { get; private set; }
        public int    Height { get; private set; }
        public int    Width  { get; private set; }
    }
}