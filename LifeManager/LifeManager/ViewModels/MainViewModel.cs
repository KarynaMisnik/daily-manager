using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LifeManager.Interfaces;
using LifeManager.Models;
using LifeManager.Services;

namespace LifeManager.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private string pageTitle = "My Tasks";
        public ObservableCollection<TodoTask> Tasks { get; } = new();
        public ObservableCollection<TodoTask> TopTasks { get; } = new();
        public ObservableCollection<TodoTask> FilteredTasks { get; } = new();
        private readonly ICategoryService _categoryService;
        public ObservableCollection<Category> Categories { get; } = new();

        public ObservableCollection<Category> FilterCategories { get; } = new();

        [ObservableProperty]
        private int selectedSortIndex = 0;

        [ObservableProperty]
        private Category? selectedCategory;
        partial void OnSelectedSortIndexChanged(int value)
        {
            UpdateFilteredTasks();
        }

        partial void OnSelectedCategoryChanged(Category? value)
        {
            UpdateFilteredTasks();
        }
        private readonly ITodoService _todoService;

        [ObservableProperty]
        private string weatherText = "Loading weather...";
        private readonly IWeatherService _weatherService;

        // constructor
        public MainViewModel(
    ITodoService todoService,
    ICategoryService categoryService,
    IWeatherService weatherService)
        {
            _todoService = todoService;
            _categoryService = categoryService;
            _weatherService = weatherService;
        }

        public async Task<HttpResponseMessage> CreateCategoryAsync(Category category)
        {
            return await _categoryService.CreateCategoryAsync(category);
        }

        [RelayCommand]
        private async Task LoadTasksAsync()
        {
            var tasks = await _todoService.GetTasksAsync();

            Tasks.Clear();

            foreach (var task in tasks)
            {
                Tasks.Add(task);
            }
            var topTasks = Tasks
    .Where(task => !task.IsCompleted)
    .OrderByDescending(task => task.Priority)
    .ThenBy(task => task.DueDate ?? DateTime.MaxValue)
    .Take(3);

            TopTasks.Clear();

            foreach (var task in topTasks)
            {
                TopTasks.Add(task);
            }
            UpdateFilteredTasks();
        }
        private void UpdateFilteredTasks()
        {
            var filteredTasks = Tasks.Where(task =>
                SelectedCategory == null
                || SelectedCategory.Id == 0
                || task.CategoryId == SelectedCategory.Id
            );

            var sortedTasks = SelectedSortIndex switch
            {
                1 => filteredTasks
                    .OrderBy(task => task.DueDate ?? DateTime.MaxValue),

                _ => filteredTasks
                    .OrderByDescending(task => task.Priority)
            };

            FilteredTasks.Clear();

            foreach (var task in sortedTasks)
            {
                FilteredTasks.Add(task);
            }
        }
        [RelayCommand]
        private async Task DeleteTaskAsync(TodoTask task)
        {
            await _todoService.DeleteTaskAsync(task.Id);
            await LoadTasksAsync();
        }
        [RelayCommand]
        private async Task ToggleTaskAsync(TodoTask task)
        {
            await _todoService.UpdateTaskAsync(task);
            await LoadTasksAsync();
        }
        public async Task<HttpResponseMessage> CreateTaskAsync(TodoTask task)
        {
            var response = await _todoService.CreateTaskAsync(task);

            if (response.IsSuccessStatusCode)
            {
                await LoadTasksAsync();
            }

            return response;
        }

        public async Task LoadCategoriesAsync()
        {
            var categories = await _categoryService.GetCategoriesAsync();

            Categories.Clear();
            FilterCategories.Clear();

            FilterCategories.Add(new Category
            {
                Id = 0,
                Name = "All categories"
            });

            foreach (var category in categories)
            {
                Categories.Add(category);
                FilterCategories.Add(category);
            }
        }

        [RelayCommand]
        private async Task LoadWeatherAsync()
        {
            WeatherText = "⏳ Loading weather...";
            try
            {
                CurrentWeather? weather = await _weatherService.GetWeatherAsync();

                if (weather != null)
                {
                    WeatherText =
                        $"Temperature: {weather.Temperature:F1}°C | Wind: {weather.WindSpeed:F1} m/s";

                    if (weather.WindSpeed > 5)
                    {
                        WeatherText += "\n🪁 Great weather for kitesurfing!";
                    }
                    else if (weather.WindSpeed >= 3)
                    {
                        WeatherText += "\n🍃 Light breeze today.";
                    }
                    else if (weather.WindSpeed >= 1)
                    {
                        WeatherText += "\n🌿 Very gentle wind.";
                    }
                    else
                    {
                        WeatherText += "\n☀️ Calm weather, almost no wind.";
                    }
                }
                else
                {
                    WeatherText = "Weather data unavailable";
                }
            }
            catch (HttpRequestException)
            {
                WeatherText = "Unable to load weather. Check your internet connection.";
            }
            catch (TaskCanceledException)
            {
                WeatherText = "Weather request timed out.";
            }
        }
    }
}