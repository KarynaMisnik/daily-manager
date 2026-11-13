using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http.Json;
using LifeManager.Models;

namespace LifeManager.Services
{
    internal class WeatherService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string WeatherApiUrl =
    "https://api.open-meteo.com/v1/forecast" +
    "?latitude=63.10" +
    "&longitude=21.62" +
    "&current=temperature_2m,wind_speed_10m" +
    "&wind_speed_unit=ms";

        public async Task<CurrentWeather?> GetWeatherAsync()
        {
            WeatherResponse? response =
                await _httpClient.GetFromJsonAsync<WeatherResponse>(WeatherApiUrl);

            return response?.Current;
        }
    }
}
