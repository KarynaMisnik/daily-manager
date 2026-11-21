using System.Net.Http.Json;
using LifeManager.Models;
using LifeManager.Interfaces;
using LifeManager.ViewModels;


namespace LifeManager;


[QueryProperty(nameof(TaskId), "taskId")]
public partial class EditTaskPage : ContentPage
{
  
    private readonly EditTaskViewModel _viewModel;
    
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

  

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.LoadCategoriesAsync();
        await _viewModel.LoadTaskAsync(int.Parse(TaskId));
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        bool saved = await _viewModel.SaveTaskAsync();

        if (!saved)
        {
            await DisplayAlertAsync(
                "Invalid task",
                "Please enter a valid title and select a category.",
                "OK"
            );
            return;
        }

        await Shell.Current.GoToAsync("..");
    }

    // constructor
    public EditTaskPage(EditTaskViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}