using NameGenerator;
using System;
using System.Collections.Generic;
using System.Text;
using Webscraper;

namespace WelcomeToChilis.Functions
{
    public static class FunctionManager
    {
        public static Generator Generator = new Generator();
        public static HirezStatusManager HirezStatusManager = new HirezStatusManager();
        public static ForecastManager ForecastManager = new ForecastManager();
        public static SteamSaleManager SteamSaleManager = new SteamSaleManager();

        public static string GetNextSteamSale()
        {
            return SteamSaleManager.GetNextSteamSale();
        }

        public static string GetIrishForecast()
        {
            return ForecastManager.GetNationalForecast();
        }

        public static string GetRocoServerStatus()
        {
            return HirezStatusManager.GetRocoServerStatus();
        }

        public static string GetAllHirezServerStatus()
        {
            return HirezStatusManager.GetAllHirezServerStatus();
        }

        public static string GetSmiteServerStatus()
        {
            return HirezStatusManager.GetSmiteServerStatus();
        }

        public static string GetName()
        {
            return Generator.MakeName();
        }

    }
}
