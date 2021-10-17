using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace Webscraper
{
    public class SteamSaleManager
    {
        private const string _steamSaleWebsite = "https://criticecho.com/steam-sale-dates/";
        private HtmlWeb Webloader { get; set; }

        public SteamSaleManager()
        {
            Webloader = new HtmlWeb();
        }

        public string GetNextSteamSale()
        {
            HtmlDocument document = Webloader.Load(_steamSaleWebsite);
            var nodes = document.DocumentNode.SelectNodes("//table[@class='event']");
            var result = nodes?[0].InnerText;

            return result;
        }
    }
}
