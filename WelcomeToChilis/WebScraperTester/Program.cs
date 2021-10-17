using System;
using Webscraper;

namespace WebScraperTester
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var forecastManager = new ForecastManager();
            var res = forecastManager.GetNationalForecast();


            var steamSale = new SteamSaleManager();
            var res2 = steamSale.GetNextSteamSale();

        }
    }
}
