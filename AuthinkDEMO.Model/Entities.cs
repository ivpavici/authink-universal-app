using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthinkDEMO.Model.Entities
{
    public class Task
    {
        public Task
        (
            int    id,
            string name,
            string description,
            string thumbUrl,
            string type,
            int    difficulty,

            IReadOnlyList<Picture> pictures,
            Sound                  voiceCommand=null

         )
        {
            this.Id          = id;
            this.Name        = name;
            this.Description = description;
            this.ThumbUrl    = thumbUrl;
            this.Type        = type;
            this.Difficulty   = difficulty;

            this.Pictures     = pictures;
            this.VoiceCommand = voiceCommand;
        }

        public int    Id          { get; private set; }
        public string Name        { get; private set; }
        public string Description { get; private set; }
        public string ThumbUrl    { get; private set; }
        public string Type        { get; private set; }
        public int    Difficulty  { get; private set; }

        public IReadOnlyList<Picture> Pictures { get; private set; }
        public Sound VoiceCommand { get; private set; }
    }
    public class Test
    {
        public Test
        (
            int    id,
            string name,
            string description,

            IReadOnlyList<Task> tasks 
         )
        {
            this.Id          = id;
            this.Name        = name;
            this.Description = description;

            this.Tasks = tasks;
        }
        
        public int    Id          { get; private set; }
        public string Name        { get; private set; }
        public string Description { get; private set; }

        public IReadOnlyList<Task> Tasks { get; private set; }
    }  
    public abstract class Picture
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public class BasicDetails: Picture
        {
            public BasicDetails
            (
                int    id,
                string url
            )
            {
                this.Id  = id;
                this.Url = url;
            }
        }
        public class AnswerPicture : Picture
        {
            public AnswerPicture() {}
            public AnswerPicture
            (
                int    id,
                string url,
                Sound  sound,
                bool   isAnswer
             )
            {
                this.Id       = id;
                this.Url      = url;
                this.Sound    = sound;
                this.IsAnswer = isAnswer;
            }
            public Sound  Sound    { get; private set; }
            public bool   IsAnswer { get; set; }
        }
        public class ColorPicture : Picture
        {
            public ColorPicture
            (
                int          id,
                string       url,
                Sound        sound,
                List<Color>  colors
             )
            {
                this.Id     = id;
                this.Url    = url;
                this.Sound  = sound;
                this.Colors = colors;
            }
            public Sound  Sound       { get; private set; }
            public List<Color> Colors { get; private set; }
        }
        public class PairHalfPicture : Picture
        {
            public PairHalfPicture
            (
                int    id,
                string url,
                string uniquepairid,
                bool   isLeftHalf
            )
            {
                this.Id           = id;
                this.Url          = url;
                this.UniquePairId = uniquepairid;
                this.IsLeftHalf = isLeftHalf;
            }
            public string UniquePairId { get; private set; }
            public bool   IsLeftHalf   { get; private set; }
        }
    }
    public class Child
    {
        public Child
        (
            int    id,
            string firstname,
            string lastname,
            string profilePictureUrl,

            IReadOnlyList<Test> tests 
        )
        {
            this.Id                = id;
            this.Firstname         = firstname;
            this.Lastname          = lastname;
            this.ProfilePictureUrl = profilePictureUrl;
            this.Tests             = tests;

        }

        public int    Id                { get; private set; }
        public string Firstname         { get; private set; }
        public string Lastname          { get; private set; }
        public string ProfilePictureUrl { get; private set; }

        public IReadOnlyList<Test> Tests { get; private set; }
    }
    public class Color
    {
        public Color
        (
            int    id,
            string value,
            bool   isCorrect,
            int    pictureId
        )
        {
            this.Id        = id;
            this.Value     = value;
            this.IsCorrect = isCorrect;
            this.PictureId = pictureId;
        }
        public int    Id        { get; private set; }
        public string Value     { get; private set; }
        public bool   IsCorrect { get; private set; }
        public int    PictureId { get; private set; }
    }
    public class Sound
    {
        public Sound
        (
            int    id,
            string url,
            string type
        )
        {
            this.Id   = id;
            this.Url  = url;
            this.Type = type;
        }
        public int    Id   { get; private set; }
        public string Url  { get; private set; }
        public string Type { get; private set; }
    }
}
