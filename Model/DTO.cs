using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PowerBIService.Model
{
    public class DTO
    {
        [JsonPropertyName("time spent in secs")]
        public double timeSpent { get; set; }
    }
}
