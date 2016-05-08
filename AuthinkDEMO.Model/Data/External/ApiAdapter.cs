using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using Newtonsoft.Json.Linq;
using Windows.Storage;

using AuthinkDEMO.Model.Data.Private;
using ent = AuthinkDEMO.Model.Entities;

namespace AuthinkDEMO.Model.Data.External
{
    using mappers = AuthinkDEMO.Model.Data.External.Mappers;
    using apiEnt  = AuthinkDEMO.Model.Data.External.ApiEntities;
    using apiRu   = AuthinkDEMO.Model.Data.External.ApiRules;

    public class ApiAdapter : IDataProvider, ILoginService
    {
        public bool Login(string username,string password)
        {
            using (var httpClient = new HttpClient())
            {
                var userLoginService_Url = string.Format("{0}{1}", "http://authink.dump.hr/", apiRu::ApiDataSource.UserLogin);

                var postMdel = new apiEnt::User
                (
                    username: username,
                    password: password
                );

                var postModel_stringContent = new StringContent(JObject.FromObject(postMdel).ToString());

                postModel_stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                httpClient.DefaultRequestHeaders.Add("AuthenticationToken", apiRu::ApiDataSource.LoginToken);

                var response = httpClient.PostAsync
                (
                    requestUri: userLoginService_Url,
                    content:    postModel_stringContent
                );
                var result = response.Result;

                return
                    result.StatusCode == HttpStatusCode.OK;
            }
        }

        private string GetAllData_byUsername(string username)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("AuthenticationToken", apiRu::ApiDataSource.ChildrenDataToken);

                var childDetails_Url = string.Format("{0}{1}{2}", "http://authink.dump.hr/", apiRu::ApiDataSource.UsersChildren, username);

                var response = httpClient.GetStringAsync(childDetails_Url);
                var result   = response.Result;

                return result;
            } 
        }

        public IEnumerable<ent::Test> GetAll()
        {
            var childId = ApplicationData.Current.LocalSettings.Values["SelectedChildId"] != null ? (int?)Convert.ToInt32(ApplicationData.Current.LocalSettings.Values["SelectedChildId"]) : null;

            return
                StorageData.GetChildData()
                           .Single(child => child.Id == childId)
                           .Tests
                           .ToList();
        }

        public IEnumerable<ent::Child> GetAll_children()
        {
            var logedInUser = ApplicationData.Current.LocalSettings.Values["Username"] as string;

            if(StorageData.GetChildData()==null)
            {
                var childData = JArray.Parse(GetAllData_byUsername(logedInUser))
                                      .Cast<JObject>()
                                      .Select(mappers::Child.FromJsonData)
                                      .ToList();

                StorageData.Setup(childData);
                return childData;
            }

            return
                StorageData.GetChildData();
        }
    }
}

namespace AuthinkDEMO.Model.Data.External.Mappers
{
    using ent   = AuthinkDEMO.Model.Entities;
    using rul   = AuthinkDEMO.Model.Rules;
    using apiRU = AuthinkDEMO.Model.Data.External.ApiRules;

    public static class Child
    {
        public static ent::Child FromJsonData(JObject childData)
        {
            return new ent::Child
            (
                id:                childData.Value<int>("Id"),
                firstname:         childData.Value<string>("Firstname"),
                lastname:          childData.Value<string>("Lastname"),
                profilePictureUrl: childData.Value<string>("ProfilePictureUrl"),

                tests:             childData["Tests"].AsJEnumerable()
                                                     .Select(jObject => Test.FromJsonData((JObject)jObject))
                                                     .ToList()
            );
        }
    }

    public static class Test
    {
        public static ent::Test FromJsonData(JObject testData)
        {
            return new ent::Test
            (
            id:          testData.Value<int>("Id"),
            name:        testData.Value<string>("Name"),
            description: testData.Value<string>("LongDescription"),

            tasks:       testData["Tasks"].AsJEnumerable()
                                          .Select(jObject => Task.FromJsoNData((JObject)jObject))
                                          .ToList()
            );
        }
    }

    public static class Task
    {
        public static ent::Task FromJsoNData(JObject taskData)
        {
            return new ent::Task
            (
            id:           taskData.Value<int>("Id"),
            description:  taskData.Value<string>("Description"),
            name:         taskData.Value<string>("Name"),
            type:         taskData.Value<string>("Type"),
            thumbUrl:     string.Format("{0}{1}", "http://authink.dump.hr/", taskData.Value<string>("ProfilePictureUrl")),

            pictures:     taskData["Pictures"].AsJEnumerable()
                                              .Select(jObject => Picture.FromJsonData((JObject)jObject, taskData.Value<string>("Type")))
                                              .ToList(),

            voiceCommand: Sound.FromJsonData(taskData.Value<JObject>("Sound")),
            difficulty:   taskData.Value<int>("Difficulty")
            );
        }
    }

    public static class Picture
    {
        public static ent::Picture FromJsonData(JObject pictureData, string taskKey)
        {
            if (rul.Task.Keys.DetectColors == taskKey)
            {
               
                return new ent::Picture.ColorPicture
                (
                    id:     pictureData.Value<int>("Id"),
                    url:    pictureData.Value<string>("Url"),

                    colors: pictureData["Colors"].AsJEnumerable()
                                                 .Select(jobject => Color.FromJsonData((JObject)jobject))
                                                 .ToList(),

                    sound:  Sound.FromJsonData(pictureData.Value<JObject>("Sound"))
                );
            } 
            if (rul.Task.Keys.PairHalves == taskKey)
            {
                return new ent::Picture.PairHalfPicture
                (
                    id:           pictureData.Value<int>("Id"),
                    url:          pictureData.Value<string>("Url"),
                    uniquepairid: "1",
                    isLeftHalf:   true
                );
            }

            return new ent::Picture.AnswerPicture
            (
                id:       pictureData.Value<int>("Id"),
                url:      pictureData.Value<string>("Url"),
                isAnswer: pictureData.Value<bool>("IsAnswer"),
                sound:    Sound.FromJsonData(pictureData.Value<JObject>("Sound"))
            );
        } 
        
    }
    public static class Sound
    {
        public static ent::Sound FromJsonData(JObject soundData)
        {
            return soundData == null ? null 
                                     : new ent::Sound
                                       (
                                           url:  soundData.Value<string>("Url"),
                                           type: soundData.Value<string>("Type"),
                                           id:   soundData.Value<int>("Id")
                                       );
        }
    }
    public static class Color
    {
        public static ent::Color FromJsonData(JObject colorData)
        {
            return colorData == null ? null : new ent::Color
                                              (
                                                value:     colorData.Value<string>("Value"),
                                                isCorrect: colorData.Value<bool>("IsCorrect"),
                                                id:        colorData.Value<int>("Id"),
                                                pictureId: colorData.Value<int>("PictureId")
                                              );
        }
    }
}
