using RssFeeder.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;
namespace RSS_Feeder.Data
{
    public static class RssFeederModel
    {
        public static  string pubDate;
        public static string title;
        public static string description;
        public static string link;
        public static List<RssItemModel> items;
        private static string _url;
        public static int _refreshTime;
        private static string _proxy;
        public static void StartRssFeeder()
        {
            items = new List<RssItemModel>();
            XmlTextReader textReader;
            SettingsModel settings = new SettingsModel();
            settings.GetSettings();
            
            if (string.IsNullOrEmpty(settings.RssUrl))
            {
                throw new Exception("Url Rss канала не задан");
            }
            else { _url = settings.RssUrl; }
            if (string.IsNullOrEmpty(settings.Proxy))
            {
                textReader = new XmlTextReader(_url);
            }
            else { _proxy = settings.Proxy;


                WebRequest request = WebRequest.Create(_url);
                WebProxy myproxy = new WebProxy(_proxy);

                if (string.IsNullOrEmpty(settings.UserName)|| string.IsNullOrEmpty(settings.Password))
                {
                    throw new Exception("Имя или пароль прокси-клиента не заданы");
                }
                else
                {
                    request.Credentials = new NetworkCredential(settings.UserName, settings.Password);
                }

                myproxy.BypassProxyOnLocal = false;
                request.Proxy = myproxy;
                request.Method = "GET";
                
                WebResponse response;
                try
                {
                    response = (WebResponse)request.GetResponse();
                }
                catch (WebException)
                {
                    throw;
                }

                
               
                textReader = new XmlTextReader(response.GetResponseStream());
            }
            _refreshTime = settings.RefreshTime;
            if (_refreshTime<=0)
            {
                _refreshTime = 60;
            }
            


            XmlDocument xmlresponse = new XmlDocument();
            try
            {
                    xmlresponse.Load(textReader);
                    textReader.Close();
                    XmlNode channelXmlNode = xmlresponse.GetElementsByTagName("channel")[0];
                    if (channelXmlNode != null)
                    {
                        foreach (XmlNode channelNode in channelXmlNode.ChildNodes)
                        {
                            switch (channelNode.Name)
                            {
                                case "title": title = channelNode.InnerText; break;
                                case "description": description = channelNode.InnerText; break;
                                case "link": link = channelNode.InnerText; break;
                                case "pubDate": link = channelNode.InnerText; break;
                                case "item":
                                    RssItemModel Item = new RssItemModel(channelNode);
                                    items.Add(Item); break;

                                default:
                                    break;
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Ошибка в XML.Описание канала не найдено!");
                    }
                }
                catch (System.Net.WebException ex)
                {

                    if (ex.Status == System.Net.WebExceptionStatus.NameResolutionFailure)
                    {
                        throw new Exception("Невозможно соединиться с указаным источником." + _url);
                    }
                    else { throw; }
                }
                catch (System.IO.FileNotFoundException)
                {
                    throw new Exception("Файл " + _url + " не найден!");
                }
                finally { textReader.Close(); }
                      
           
        }
    }
}
