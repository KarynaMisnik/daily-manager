using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http.Json;
using LifeManager.Models;
using LifeManager.Interfaces;

namespace LifeManager.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
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
