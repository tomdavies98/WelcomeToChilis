using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Webscraper
{
    public class ForecastManager
    {
        private string _forecastUrl = "https://www.met.ie/forecasts/national-forecast";
        private HtmlWeb Webloader { get; set; }

        public ForecastManager()
        {
            Webloader = new HtmlWeb();
        }

        public string GetNationalForecast()
        {
            HtmlDocument document = Webloader.Load(_forecastUrl);
            var nodes = document.DocumentNode.SelectNodes("//div[@class='forecast']");
            var words = nodes[0].InnerText;
            var res = Regex.Replace(words, @"(\s)\1+", "$1");
            res = res.Substring(0, res.ToLower().IndexOf("tonight"));
            return res;
        }


    }
}
