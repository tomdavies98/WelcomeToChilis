using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Webscraper
{
    public class HirezStatusManager
    {
        private const string HirezUrl = "http://status.hirezstudios.com/";
        private HtmlWeb Webloader { get; set; }
        public HirezStatusManager()
        {
            Webloader = new HtmlWeb();
        }

        private List<string> RemoveUselessEntries(List<string> entries)
        {
            var usefulEntries = new List<string>();

            for (int i = 0; i < entries.Count; ++i)
            {
                if (!string.IsNullOrWhiteSpace(entries[i]) && !entries[i].Contains("?"))
                {
                    usefulEntries.Add(entries[i]);
                }
            }

            return usefulEntries;
        }

        private Dictionary<string, string> MapGameToStatus(List<string> gameStatus)
        {
            var gameStatusDict = new Dictionary<string, string>();

            for(int i =0; i< gameStatus.Count;++i)
            {
                try
                {
                    var key = gameStatus[i].Trim();

                    if(!gameStatusDict.ContainsKey(key))
                        gameStatusDict.Add(key, gameStatus[i + 1].Trim());
                }
                catch (Exception e)
                {
                    Console.WriteLine($"An error has occured: {e}");
                }
            }

            return gameStatusDict;
        }

        private Dictionary<string,string> GetHirezOperationDetails()
        {
            var gameStatusDict = new Dictionary<string, string>();
            try
            {
                HtmlDocument document = Webloader.Load(HirezUrl);
                var nodes = document.DocumentNode.SelectNodes("//div[@class='components-container one-column']");
                var words = nodes[0].InnerText.Split('\n');
                var entries = RemoveUselessEntries(words.ToList<string>());
                gameStatusDict = MapGameToStatus(entries);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error occured while collecting Hirez server data");
            }
            

            return gameStatusDict;
        }

        public string GetAllHirezServerStatus()
        {
            var gameStatusDict = GetHirezOperationDetails();

            var prettyOutput = "";
            foreach(var entry in gameStatusDict)
            {
                prettyOutput += entry.Key + ": " + entry.Value + "\n";
            }

            return prettyOutput;
        }

        public string GetRocoServerStatus()
        {
            var gameStatusDict = GetHirezOperationDetails();
            var rogueCompanyStatus = gameStatusDict["Rogue Company"];

            return rogueCompanyStatus;
        }

        public string GetSmiteServerStatus()
        {
            var gameStatusDict = GetHirezOperationDetails();
            var smiteStatus = gameStatusDict["SMITE"];

            return smiteStatus;
        }

    }
}
