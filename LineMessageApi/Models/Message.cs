using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LineMessageApi.Models
{
    public class Message
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
