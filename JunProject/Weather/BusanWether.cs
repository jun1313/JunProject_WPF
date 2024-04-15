using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using static System.Net.WebRequestMethods;

namespace JunProject.Weather
{
    public class BusanWether
    {
        string uri = "https://www.weather.go.kr/w/rss/dfs/hr1-forecast.do?zone=2644053500";


        public BusanWether() 
        {
            Weathers = new ObservableCollection<WeatherInfo>();
        }

        public ObservableCollection<WeatherInfo> Weathers { get; set; }
        public string WeatherPosition {  get; set; }


        public async Task UpdateWeatherAsync()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(uri);// HTTP GET 요청을 보냄 Task<HttpResponseMessage>형식
                    response.EnsureSuccessStatusCode();//성공처리여부 확인
                    var xmlData = await response.Content.ReadAsStringAsync();//Task<string> await하여 실제문자열 데이터 얻음
                    var xDoc = new XmlDocument();
                    xDoc.LoadXml(xmlData);
                    XmlNodeList tagNodes = xDoc.SelectNodes("//channel/item/description/body/data");
                    XmlNodeList tagNodes_locate = xDoc.SelectNodes("//channel/item");
                    WeatherPosition = tagNodes_locate[0].SelectSingleNode("category").InnerText;
                    for(int i = 0; i < 9; i++)
                    {
                        Weathers.Add(insert(tagNodes, 3*i));
                    }
                }
            }
            catch (Exception ex)
            {
                // 오류 처리
                MessageBox.Show($"오류 발생: {ex.Message}");
            }

        }

        public WeatherInfo insert(XmlNodeList tagNodes , int num)
        {
            WeatherInfo temp = new WeatherInfo();
            temp.Time = tagNodes[num].SelectSingleNode("hour").InnerText + "시";
            temp.Weather = tagNodes[num].SelectSingleNode("wfKor").InnerText.Replace(" ", "");
            temp.Temperature = tagNodes[num].SelectSingleNode("temp").InnerText + "ºC";
            if (temp.Weather.Contains("맑음")) temp.Image = "img/sun.png";
            else if (temp.Weather.Contains("흐림")) temp.Image = "img/sunwithcloud.png";
            else if (temp.Weather.Contains("구름")) temp.Image = "img/cloud.png";
            else if (temp.Weather.Contains("비")) temp.Image = "img/rain.png";
            return temp;
        }

    }
    public class WeatherInfo
    {
        public string Image { get; set; }
        public string Time { get; set; }
        public string Weather { get; set; }
        public string Temperature { get; set; }
    }

}
