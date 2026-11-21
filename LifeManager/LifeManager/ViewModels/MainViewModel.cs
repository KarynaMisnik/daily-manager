using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using LifeManager.Models;
using LifeManager.Interfaces;
using CommunityToolkit.Mvvm.Input;

namespace LifeManager.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private string pageTitle = "My Tasks";
        public ObservableCollection<TodoTask> Tasks { get; } = new();
        public ObservableCollection<TodoTask> TopTasks { get; } = new();
        public ObservableCollection<TodoTask> FilteredTasks { get; } = new();

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

        public MainViewModel(ITodoService todoService)
        {
            _todoService = todoService;
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
    }
}