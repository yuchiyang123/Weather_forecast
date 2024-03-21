using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using static Weather_forecast.Controllers.IndexController;

namespace Weather_forecast.Controllers
{
    public class IndexController : Controller
    {
        [HttpPost]
        public async Task<ActionResult> Weather_Api(string loc)
        {
            ViewBag.loc = loc;
            using (var client = new HttpClient())
            {
                //API
                //string apiUrl = "https://opendata.cwa.gov.tw/api/v1/rest/datastore/F-C0032-001?Authorization=CWA-5072064D-529F-4F6D-8AF1-205945EE29CF&limit=20&offset=0&format=JSON";
                string apiUrl = "https://opendata.cwa.gov.tw/api/v1/rest/datastore/F-C0032-001?Authorization=CWA-5072064D-529F-4F6D-8AF1-205945EE29CF&locationName=&elementName=";
                //跟API請求資料
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                //響應是否成功
                if (response.IsSuccessStatusCode)
                {
                    //解析JSON
                    string jsonResult = await response.Content.ReadAsStringAsync();


                    //Console.WriteLine(jsonResult);
                    //協帶資料傳回視圖
                    //ViewBag.JsonResult = jsonResult;
                    //將json解析為JObject
                    var jsonObject = JObject.Parse(jsonResult);

                    //地名檢查 ex.台北市
                    var locationArray = (JArray)jsonObject["records"]["location"];

                    List<WeatherData> weatherDataList = new List<WeatherData>();
                    List<WeatherDataPoP> weatherDataListPoP = new List<WeatherDataPoP>();
                    List<WeatherDataMinT> weatherDataListMinT = new List<WeatherDataMinT>();
                    List<WeatherDataMaxT> weatherDataListMaxT = new List<WeatherDataMaxT>();
                    List<WeatherDataCI> weatherDataListCI = new List<WeatherDataCI>();
                    foreach (var location in locationArray)
                    {
                        var locationName = (string)location["locationName"];
                        //選擇的地名是否一樣
                        if (loc == locationName)
                        {

                            //var elementNameArray = (JArray)location["elementName"];
                            //foreach (var elementN in elementNameArray)
                            //{
                            var elementABC = (JArray)location["weatherElement"];
                            foreach (var element in elementABC)
                            {
                                var elementName = (string)element["elementName"];
                                //Wx 晴天雨天狀況說明 ex.陰時多雲短暫陣雨
                                if (elementName == "Wx")
                                {
                                    //var timeArray = (JArray)location["elementName"]["time"];
                                    //foreach (var time in timeArray)
                                    //{
                                    var timeArray = (JArray)element["time"];
                                    foreach (var time in timeArray)
                                    {
                                        string startTime = (string)time["startTime"];
                                        string endTime = (string)time["endTime"];
                                        string parameterName = (string)time["parameter"]["parameterName"];
                                        string parameterValue = (string)time["parameter"]["parameterValue"];

                                        
                                        WeatherData weatherData = new WeatherData(startTime, endTime, parameterName, parameterValue);
                                        weatherDataList.Add(weatherData);
                                    }

                                    
                                    //}
                                }
                                if (elementName == "PoP")
                                {
                                    //var PoPtimeArray = (JArray)location["elementName"]["time"];
                                    //foreach (var PoPtime in PoPtimeArray)
                                    //{
                                    var timeArrayPoP = (JArray)element["time"];
                                    foreach (var timePoP in timeArrayPoP)
                                    {
                                        string parameterNamePoP = (string)timePoP["parameter"]["parameterName"];
                                        string parameterUnitPoP = (string)timePoP["parameter"]["parameterUnit"];

                                        WeatherDataPoP weatherDataPoP = new WeatherDataPoP(parameterNamePoP, parameterUnitPoP);
                                        weatherDataListPoP.Add(weatherDataPoP);
                                    }
                                    //}
                                }
                                if (elementName == "MinT")
                                {
                                    //var MinTtimeArray = (JArray)location["elementName"]["time"];
                                    //foreach (var MinTtime in MinTtimeArray)
                                    //{
                                    var timeArrayMinT = (JArray)element["time"];
                                    foreach (var timeMinT in timeArrayMinT)
                                    {
                                        string parameterNameMinT = (string)timeMinT["parameter"]["parameterName"];
                                        string parameterUnitMinT = (string)timeMinT["parameter"]["parameterUnit"];

                                        WeatherDataMinT weatherDataMinT = new WeatherDataMinT(parameterNameMinT, parameterUnitMinT);
                                        weatherDataListMinT.Add(weatherDataMinT);
                                    }
                                    //}
                                }
                                if (elementName == "MaxT")
                                {
                                    //var MaxTtimeArray = (JArray)location["elementName"]["time"];
                                    //foreach (var MaxTtime in MaxTtimeArray)
                                    //{
                                    var timeArrayMaxT = (JArray)element["time"];
                                    foreach (var timeMaxT in timeArrayMaxT)
                                    {
                                        string parameterNameMaxT = (string)timeMaxT["parameter"]["parameterName"];
                                        string parameterUnitMaxT = (string)timeMaxT["parameter"]["parameterUnit"];

                                        WeatherDataMaxT weatherDataMaxT = new WeatherDataMaxT(parameterNameMaxT, parameterUnitMaxT);
                                        weatherDataListMaxT.Add(weatherDataMaxT);
                                    }
                                    //}
                                }
                                if (elementName == "CI")
                                {
                                    //var CItimeArray = (JArray)location["elementName"]["time"];
                                    //foreach (var CItime in CItimeArray)
                                    //{
                                    var timeArrayCI = (JArray)element["time"];
                                    foreach (var timeCI in timeArrayCI)
                                    {
                                        string parameterNameCI = (string)timeCI["parameter"]["parameterName"];

                                        WeatherDataCI weatherDataCI = new WeatherDataCI(parameterNameCI);
                                        weatherDataListCI.Add(weatherDataCI);
                                        //}
                                    }
                                }
                            }
                                
                            //}



                        }
                    }
                    ViewBag.WeatherDataList = weatherDataList;
                    ViewBag.WeatherDataListPoP = weatherDataListPoP;
                    ViewBag.WeatherDataListMinT = weatherDataListMinT;
                    ViewBag.WeatherDataListMaxT = weatherDataListMaxT;
                    ViewBag.WeatherDataListCI = weatherDataListCI;
                    DateTime currenttime = DateTime.Now;
                    ViewBag.Currenttime = currenttime;
                    string dataPart = currenttime.ToString("dd");
                    int dataPartint = int.Parse(dataPart.ToString()); 
                    ViewBag.Currenttimedd = dataPartint;
                    return View("Index");


                }
                else
                {
                    return View("Index");
                }
            }
        }
        // GET: Index
        public ActionResult Index()
        {
            return View();
        }
        /*public ActionResult Weather_handling() 
        { 
            
        
        
        }*/

        [HttpPost]
        public ActionResult WeatherHandling(string city)
        {
            
            return View("Index");
        }

        public class WeatherData
        {
            public string StartTime { get; set; }
            public string EndTime { get; set; }
            public string ParameterName { get; set; }
            public string ParameterValue { get; set; }

            public WeatherData(string startTime, string endTime, string parameterName, string parameterValue)
            {
                StartTime = startTime;
                EndTime = endTime;
                ParameterName = parameterName;
                ParameterValue = parameterValue;
            }

        }
        public class WeatherDataPoP
        {
            public string ParameterNamePoP { get; set; }
            public string ParameterUnitPoP { get; set; }

            public WeatherDataPoP(string parameterNamePoP, string parameterUnitPoP)
            {
                ParameterNamePoP = parameterNamePoP;
                ParameterUnitPoP = parameterUnitPoP;
            }
        }

        public class WeatherDataMinT
        {
            public string ParameterNameMinT { get; set; }
            public string ParameterUnitMinT { get; set; }
            public WeatherDataMinT(string parameterNameMinT, string parameterUnitMinT)
            {
                ParameterNameMinT = parameterNameMinT;
                ParameterUnitMinT = parameterUnitMinT;
            }
        }

        public class WeatherDataMaxT
        {
            public string ParameterNameMaxT { get; set; }
            public string ParameterUnitMaxT { get; set; }
            public WeatherDataMaxT(string parameterNameMaxT, string parameterUnitMaxT)
            {
                ParameterNameMaxT = parameterNameMaxT;
                ParameterUnitMaxT = parameterUnitMaxT;
            }
        }

        public class WeatherDataCI
        {
            public string ParameterNameCI { get; set; }
            public WeatherDataCI(string parameterNameCI)
            {
                ParameterNameCI = parameterNameCI;

            }
        }


    }
}