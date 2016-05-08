using System;
using System.Collections.ObjectModel;
using AuthinkDEMO.Views.GameViews.PairHalfsTask;
using GalaSoft.MvvmLight.Command;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using System.Linq;

using GalaSoft.MvvmLight;

using ent = AuthinkDEMO.Model.Entities;
using AuthinkDEMO.Model.Queries;
using AuthinkDEMO.Services;
using System.Collections.Generic;

namespace AuthinkDEMO.ViewModel.GameViewModels
{
    public partial class PairHalfsViewModel : ViewModelBase
    {
        async private void TransformPicturesDataToModelData(List<ent::Picture.PairHalfPicture> picturesData)
        {
            foreach (var picture in picturesData)
            {
                var halves = await PictureService.GetHalves(picture);

                this.PairPictureCollection.Add(new PairHalfsPicture(picture.Id, halves.Item2, halves.Item1, picture.Url, picture.Id.ToString()));

            }

            this.PairPictureCollection.Shuffle();
        }
        private void Init()
        {
            var pictures = pictureQueries.GetAllPicturesForTask(GameState.GetTask())
                                         .Select(picture => (ent::Picture.PairHalfPicture)picture)
                                         .ToList();

            SoundUrl = SoundServices.GetInstructionsSoundUrl
            (
                sound: taskQueries.GetSingle_byId(GameState.GetTask()).VoiceCommand
            );

            TransformPicturesDataToModelData
            (
                picturesData: pictures
            );
            PictureCount = pictures.Count();
        }
        
    }

    public partial class PairHalfsViewModel
    {
        public PairHalfsViewModel
        (
            ITaskQueries    taskQueries,
            IPictureQueries pictureQueries
        )
        {
            this.pictureQueries = pictureQueries;
            this.taskQueries    = taskQueries;
            this.PairPictureCollection = new ObservableCollection<PairHalfsPicture>();
            
            Init();
        }

        private readonly ITaskQueries    taskQueries;
        private readonly IPictureQueries pictureQueries;

        public int PictureCount { get; set; }
        public ObservableCollection<PairHalfsPicture> PairPictureCollection { get; set; }
        public Uri SoundUrl { get; set; }
    }

    public class PairHalfsPicture
    {
        public PairHalfsPicture
        (
            int id,
            ImageSource rightImageSource,
            ImageSource leftImageSource,
            string wholeImageSource,
            string uniquepairid
           
        )
        {
            this.Id = id;
            this.RightImageSource = rightImageSource;
            this.LeftImageSource = leftImageSource;
            this.WholeImageSource = wholeImageSource;
            this.UniquePairId = uniquepairid;
        }
        public int Id { get; private set; }
        public ImageSource RightImageSource { get; private set; }
        public ImageSource LeftImageSource { get; private set; }
        public string WholeImageSource { get; set; }
        public string UniquePairId { get; private set; }
    }

   
}