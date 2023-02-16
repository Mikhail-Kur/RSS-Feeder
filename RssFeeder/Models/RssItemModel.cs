using System.Xml;

namespace RSS_Feeder.Data
{
    public class RssItemModel
    {
        public string title;
        public string description;
        public string link;
        public string pubDate;
        
        public RssItemModel(XmlNode itemTag)
        {

            foreach (XmlNode xmlTag in itemTag.ChildNodes)
            {
                switch (xmlTag.Name)
                {
                    case ("title"):
                        this.title = xmlTag.InnerText;
                        break;
                    case ("description"):
                        this.description = xmlTag.InnerText;
                        break;
                    case ("link"):
                        this.link = xmlTag.InnerText;
                        break;
                    case ("pubDate"):
                        this.pubDate = xmlTag.InnerText;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
