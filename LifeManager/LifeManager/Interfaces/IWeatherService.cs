using LifeManager.Models;

namespace LifeManager.Interfaces
{
    public interface IWeatherService
    {
        Task<CurrentWeather?> GetWeatherAsync();
    }
}