using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Hosting.Providers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace RedditMemeManager
{
    public static class MemeManager
    {
        private static Random _rand = new Random();

        public static string GetMeme(string memeFolderPath)
        {
            var memesPath = new List<string>();
            string[] fileEntries = Directory.GetFiles(memeFolderPath);

            if (fileEntries.Length <= 0)
            {
                return null;
            }

            foreach (string fileName in fileEntries)
            {
                var length = new System.IO.FileInfo(fileName).Length;
                if (!fileName.Contains(".py") && length > 0)
                {
                    memesPath.Add(fileName);
                }
            }

            //return random meme
            return memesPath[_rand.Next(memesPath.Count)];
        }

        public static bool DeleteMeme(string memeToDelete)
        {
            try
            {
                File.Delete(memeToDelete);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return false;
        }
    }
}
