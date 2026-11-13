using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LifeManager.Models
{
    internal class CurrentWeather
    {
        [JsonPropertyName("temperature_2m")]
        public double Temperature { get; set; }

        [JsonPropertyName("wind_speed_10m")]
        public double WindSpeed { get; set; }
    }
}
