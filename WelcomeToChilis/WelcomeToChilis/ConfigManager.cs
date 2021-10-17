using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WelcomeToChilis
{
    public static class ConfigManager
    {
        private static ConfigJson _configJson { get; set; }

        public static ConfigJson GetConfig()
        {
            if (_configJson.Token == null)
            {
                var json = string.Empty;

                using (var fs = File.OpenRead("config.json"))
                {
                    using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                    {
                        json = sr.ReadToEnd();
                    }
                }

                _configJson = JsonConvert.DeserializeObject<ConfigJson>(json);
            }
           
            return _configJson;
        }
    }
}
