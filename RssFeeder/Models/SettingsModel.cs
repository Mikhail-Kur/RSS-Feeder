using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace RssFeeder.Models
{
    public class SettingsModel
    {
        [Required]
        [Display(Name = "Сылка на Rss канал")]
        public string RssUrl;
        
        [Display(Name = "Прокси сервер")]
        public string Proxy;
        
        [Display(Name = "Частота обновления(сек)")]
        public int RefreshTime;
        [Required]
        [Display(Name = "Имя пользователя")]
        public string UserName;
        [Required]
        [Display(Name = "Пароль")]
        public string Password;
        public void GetSettings()
        {
            
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("Settings.xml");
            XmlNode SettingsNode = xDoc.GetElementsByTagName("Setting")[0];
            foreach (XmlNode node in SettingsNode.ChildNodes)
            {
                switch (node.Name)
                {
                    case "Url": RssUrl = node.Attributes["Url"].Value; break;
                    case "Proxy": Proxy = node.Attributes["Proxy"].Value; break;
                    case "RefreshTime": RefreshTime = Int32.Parse(node.Attributes["RefreshTime"].Value); break;
                    case "UserName": UserName = node.Attributes["UserName"].Value; break;
                    case "Password": Password = node.Attributes["Password"].Value; break;
                    default:
                        break;
                }
            }

           
        }
    }
}   
