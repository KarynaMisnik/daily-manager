using System.Net.Http.Json;
using LifeManager.Models;
using LifeManager.Interfaces;


namespace LifeManager;


[QueryProperty(nameof(TaskId), "taskId")]
public partial class EditTaskPage : ContentPage
{
    private readonly ITodoService _todoService;
    private readonly ICategoryService _categoryService;
    private TodoTask? _currentTask;
    private string _taskId = string.Empty;

    public string TaskId
    {
        get => _taskId;
        set
        {
            _taskId = value;
            System.Diagnostics.Debug.WriteLine($"Editing task ID: {_taskId}");
        }
    }

    private async Task LoadCategoriesAsync()
    {
        var categories = await _categoryService.GetCategoriesAsync();

        CategoryPicker.ItemsSource = categories;
    }

    private async Task LoadTaskAsync()
    {
        TodoTask? task = await _todoService.GetTaskByIdAsync(int.Parse(TaskId));

        if (task != null)
        {
            _currentTask = task;
            TitleEntry.Text = task.Title;
            PriorityPicker.SelectedIndex = task.Priority - 1;
            DueDatePicker.Date = task.DueDate;
            CategoryPicker.SelectedItem =
    (CategoryPicker.ItemsSource as IEnumerable<Category>)?
        .FirstOrDefault(c => c.Id == task.CategoryId);


        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadCategoriesAsync();
        await LoadTaskAsync();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (_currentTask == null)
            return;

        string newTitle = TitleEntry.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(newTitle))
        {
            await DisplayAlertAsync(
                "Invalid title",
                "Task title cannot be empty.",
                "OK"
            );
            return;
        }

        _currentTask.Title = newTitle;
        _currentTask.Priority = PriorityPicker.SelectedIndex + 1;

        _currentTask.DueDate = DueDatePicker.Date.HasValue
            ? DateTime.SpecifyKind(DueDatePicker.Date.Value, DateTimeKind.Utc)
            : null;

        _currentTask.CategoryId = ((Category)CategoryPicker.SelectedItem).Id;

        await _todoService.UpdateTaskAsync(_currentTask);

        await Shell.Current.GoToAsync("..");
    }

    // constructor
    public EditTaskPage(
     ITodoService todoService,
     ICategoryService categoryService)
    {
        InitializeComponent();

        _todoService = todoService;
        _categoryService = categoryService;
    }
}