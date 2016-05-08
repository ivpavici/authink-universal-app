using System;
using System.Collections.ObjectModel;
using AuthinkDEMO.Model.Queries;
using AuthinkDEMO.Services;
using System.Linq;
using GalaSoft.MvvmLight;

using ent = AuthinkDEMO.Model.Entities;
using Windows.Storage;
using Windows.UI.Xaml;
using AuthinkDEMO.Views;

namespace AuthinkDEMO.ViewModel.GameViewModels
{
    public partial class PairSameItemsViewModel : ViewModelBase
    {
        public PairSameItemsViewModel
        (
            IPictureQueries pictureQueries,
            ITaskQueries taskQueries,
            NavigationService navigationService
        )
        {
            this.pictureQueries = pictureQueries;
            _taskQueries = taskQueries;
            this.navigationService = navigationService;

            this.Items_left            = new ObservableCollection<PairSameItemsPicture>();
            this.Items_right           = new ObservableCollection<PairSameItemsPicture>();
            this.Items_left_empty      = new ObservableCollection<PairSameItemsPicture>();
            this.Items_right_empty     = new ObservableCollection<PairSameItemsPicture>();
            this.ItemsSelectionList    = new ObservableCollection<PairSameItemsPicture>();
            Transform_TaskData_To_ViewModelData();

        }
        private readonly ITaskQueries _taskQueries;
        private readonly IPictureQueries pictureQueries;
        private readonly NavigationService navigationService;
        public ObservableCollection<PairSameItemsPicture> Items_left { get; private set; }
        public ObservableCollection<PairSameItemsPicture> Items_left_empty { get; private set; }
                        
        public ObservableCollection<PairSameItemsPicture> Items_right { get; private set; }
        public ObservableCollection<PairSameItemsPicture> Items_right_empty { get; private set; }
                      
        public ObservableCollection<PairSameItemsPicture> ItemsSelectionList { get; set; }
        public Uri SoundUrl { get; set; }
        
        
    }

    public partial class PairSameItemsViewModel
    {
       
        private void Transform_TaskData_To_ViewModelData()
        {
            var pictures = pictureQueries.GetAllPicturesForTask(GameState.GetTask())
                .Select(picture => (ent::Picture.AnswerPicture)picture)
                                         .ToList();

            var sound = _taskQueries.GetSingle_byId(GameState.GetTask()).VoiceCommand;

            SoundUrl = sound != null && (bool)ApplicationData.Current.LocalSettings.Values["IsInstructionSoundEnabled"] ? new Uri(sound.Url) : null;

            foreach (var picture in pictures)
            {
                var currentPictureIndex = pictures.ToList().IndexOf(picture);

                ItemsSelectionList.Add(new PairSameItemsPicture(picture.Id, picture.Url,currentPictureIndex.ToString()));
                
                if (currentPictureIndex < 3)
                {
                    this.Items_left_empty.Add(new PairSameItemsPicture(picture.Id, picture.Url, currentPictureIndex.ToString()));
                }
                else if (currentPictureIndex >= 3)
                {
                    this.Items_right_empty.Add(new PairSameItemsPicture(picture.Id, picture.Url, currentPictureIndex.ToString()));
                }
            }

            foreach (var picture in pictures)
            {
                var currentPictureIndex = pictures.ToList().IndexOf(picture);

                if (currentPictureIndex < 3)
                {
                    this.Items_left.Add
                    (
                        new PairSameItemsPicture(picture.Id, picture.Url, currentPictureIndex.ToString())
                    );

                }
                else if (currentPictureIndex >= 3)
                {
                    this.Items_right.Add
                    (
                       new PairSameItemsPicture(picture.Id, picture.Url, currentPictureIndex.ToString())
                    );
                }
            }
            this.ItemsSelectionList.Shuffle();
        }
    }
    public class PairSameItemsPicture
    {
        public PairSameItemsPicture
        (
            int id,
            string url,
            string uniquepairid
        )
        {
            this.Id = id;
            this.Url = url;
            this.UniquePairId = uniquepairid;
        }

        public int    Id           { get; private set; }
        public string Url          { get; private set; }
        public string UniquePairId { get; private set; }
    }
}