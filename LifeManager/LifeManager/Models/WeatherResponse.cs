using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LifeManager.Models
{
    internal class WeatherResponse
    {
        [JsonPropertyName("current")]
        public CurrentWeather? Current { get; set; }
    }
}
