using System;
using Webscraper;

namespace HirezTester
{
    public class Program
    {
        public static void Main(string[] args)
        {
            HirezStatusManager web = new HirezStatusManager();
            var res = web.GetAllHirezServerStatus();
        }
    }
}
