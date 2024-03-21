using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Weather_forecast.Controllers
{
    public class YoutubeController : Controller
    {
        // GET: Youtube
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Handle(string selectOptionvalue)
        {
            return Redirect("/" + selectOptionvalue);
        }
        [HttpPost]
        public async Task<ActionResult> Youtube_api(string keyword)
        {
            ViewBag.keyword = keyword;
            using (var client = new HttpClient())
            {
                string api = "https://www.googleapis.com/youtube/v3/search?" +
                    "q="+keyword +
                    "&key=AIzaSyCt9ktHtjL8spHSy3Iob1LlvR1LR1p8xlI" +
                    "&part=snippet&type=video";
                HttpResponseMessage response = await client.GetAsync(api);
                if (response.IsSuccessStatusCode)
                {
                    List<YoutubeData>youtubeDataslist = new List<YoutubeData>();
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    var jsonObject = JObject.Parse(jsonResult);
                    var jsonArray = (JArray)jsonObject["items"];
                    foreach (var json in jsonArray)
                    {
                        string kind = (string)json["id"]["kind"];
                        string videoId = (string)json["id"]["videoId"];
                        string publishedAt = (string)json["snippet"]["publishedAt"];
                        string title = (string)json["snippet"]["title"];
                        string description = (string)json["snippet"]["description"];
                        string videoPhoto = (string)json["snippet"]["thumbnails"]["high"]["url"];
                        int videoPhotoWidth = (int)json["snippet"]["thumbnails"]["high"]["width"];
                        int videoPhotohigh = (int)json["snippet"]["thumbnails"]["high"]["height"];
                        string chennelTitle = (string)json["chennelTitle"];
                        YoutubeData youtubeData = new YoutubeData(kind, videoId, publishedAt, title,
                            description, videoPhoto, videoPhotoWidth, videoPhotohigh, chennelTitle);
                        youtubeDataslist.Add(youtubeData);
                    }
                    Console.Write(youtubeDataslist);
                    return View("Index");
                }
                else
                {
                    return View("error");
                }

            }
        }
        //資料型態定義
        public class YoutubeData
        {
            public string kind { get; set; }
            public string videoId { get; set; }
            public string publishedAt { get; set; }
            public string title { get; set; }

            public string description { get; set; }
            public string videoPhoto { get; set; }

            public int videoPhotoWidth { get; set; }
            public int videoPhotoHeight { get; set; }

            public string chennelTitle { get; set; }
            public YoutubeData(string kind, string videoId, string publishedAt, string title, string description, string videoPhoto, int videoPhotoWidth, int videoPhotoHeight, string chennelTitle)
            {
                this.kind = kind;
                this.videoId = videoId;
                this.publishedAt = publishedAt;
                this.title = title;
                this.description = description;
                this.videoPhoto = videoPhoto;
                this.videoPhotoWidth = videoPhotoWidth;
                this.videoPhotoHeight = videoPhotoHeight;
                this.chennelTitle = chennelTitle;
            }
        }
    }
}