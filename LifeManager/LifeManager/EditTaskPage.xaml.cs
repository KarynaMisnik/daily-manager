using System.Net.Http.Json;
using LifeManager.Models;

namespace LifeManager;


[QueryProperty(nameof(TaskId), "taskId")]
public partial class EditTaskPage : ContentPage
{
    private readonly HttpClient _httpClient = new HttpClient();
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

    private async Task LoadTaskAsync()
    {
        TodoTask? task = await _httpClient.GetFromJsonAsync<TodoTask>(
            $"https://localhost:7187/api/tasks/{TaskId}"
        );

        if (task != null)
        {
            _currentTask = task;
            TitleEntry.Text = task.Title;
            PriorityPicker.SelectedIndex = task.Priority - 1;
            DueDatePicker.Date = task.DueDate;
            CategoryPicker.SelectedIndex = task.CategoryId == 1 ? 0 : -1;


        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

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

        // General is currently our only available category.
        _currentTask.CategoryId = 1;

        var response = await _httpClient.PutAsJsonAsync(
            $"https://localhost:7187/api/tasks/{_currentTask.Id}",
            _currentTask
        );

        response.EnsureSuccessStatusCode();

        await Shell.Current.GoToAsync("..");
    }
    public EditTaskPage()
	{
		InitializeComponent();
	}
}