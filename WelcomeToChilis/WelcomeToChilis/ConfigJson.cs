using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeToChilis
{
    public struct ConfigJson
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("prefix")]
        public string Prefix { get; set; }

        [JsonProperty("memeFolderPath")]
        public string MemeFolderPath { get; set; }
    }
}
