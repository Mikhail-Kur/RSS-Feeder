using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RSS_Feeder.Data;
using RssFeeder.Models;
using System.Diagnostics;
using System.Xml;


namespace RssFeeder.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Rss()
        {
            RssFeederModel.StartRssFeeder();
            ViewBag.RefreshTime = RssFeederModel._refreshTime;
            ViewData.Model = RssFeederModel.items;

            return View();
        }

        [HttpGet]
        public IActionResult Settings()
        {

            return View();
        } 
        [HttpPost]
        public IActionResult Settings(string RssUrl, string Proxy, int RefreshTime,string UserName, string Password) {
            if (string.IsNullOrEmpty(RssUrl))
            {
                ModelState.AddModelError("RssUrl", "Введите Url адрес");
            }
            if (string.IsNullOrEmpty(UserName))
            {
                ModelState.AddModelError("UserName", "Введите имя клиента");
            }
            if (string.IsNullOrEmpty(Password))
            {
                ModelState.AddModelError("Password", "Введите пароль");
            }
            if (ModelState.IsValid)
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load("Settings.xml");
                XmlNode SettingsNode = xDoc.GetElementsByTagName("Setting")[0];
                foreach (XmlNode node in SettingsNode.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "Url": node.Attributes["Url"].Value = RssUrl; break;
                        case "Proxy": node.Attributes["Proxy"].Value = Proxy; break;
                        case "RefreshTime": node.Attributes["RefreshTime"].Value = RefreshTime.ToString(); break;
                        default:
                            break;
                    }
                }
                xDoc.Save("Settings.xml");
                return RedirectToPage("/Rss");
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
