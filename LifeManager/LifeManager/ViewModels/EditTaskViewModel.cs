using CommunityToolkit.Mvvm.ComponentModel;
using LifeManager.Interfaces;
using LifeManager.Models;
using System.Collections.ObjectModel;

namespace LifeManager.ViewModels;

public partial class EditTaskViewModel : ObservableObject
{
    private readonly ITodoService _todoService;
    private readonly ICategoryService _categoryService;

    [ObservableProperty]
    private string taskTitle = string.Empty;

    [ObservableProperty]
    private int selectedPriorityIndex = 0;

    [ObservableProperty]
    private DateTime? dueDate;

    [ObservableProperty]
    private Category? selectedCategory;

    public ObservableCollection<Category> Categories { get; } = new();

    private TodoTask? _currentTask;

    public EditTaskViewModel(
        ITodoService todoService,
        ICategoryService categoryService)
    {
        _todoService = todoService;
        _categoryService = categoryService;
    }

    public async Task LoadCategoriesAsync()
    {
        var categories = await _categoryService.GetCategoriesAsync();

        Categories.Clear();

        foreach (var category in categories)
        {
            Categories.Add(category);
        }
    }

    public async Task LoadTaskAsync(int taskId)
    {
        var task = await _todoService.GetTaskByIdAsync(taskId);

        if (task == null)
            return;

        _currentTask = task;

        TaskTitle = task.Title;
        SelectedPriorityIndex = task.Priority - 1;
        DueDate = task.DueDate;

        SelectedCategory = Categories
            .FirstOrDefault(c => c.Id == task.CategoryId);
    }
    public async Task<bool> SaveTaskAsync()
    {
        if (_currentTask == null)
            return false;

        string newTitle = TaskTitle.Trim();

        if (string.IsNullOrWhiteSpace(newTitle) || SelectedCategory == null)
            return false;

        _currentTask.Title = newTitle;
        _currentTask.Priority = SelectedPriorityIndex + 1;

        _currentTask.DueDate = DueDate.HasValue
            ? DateTime.SpecifyKind(DueDate.Value, DateTimeKind.Utc)
            : null;

        _currentTask.CategoryId = SelectedCategory.Id;

        await _todoService.UpdateTaskAsync(_currentTask);

        return true;
    }
}