
using System.Net.Http.Json;
using LifeManager.Services;
using LifeManager.Models;

namespace LifeManager
{
    public partial class MainPage : ContentPage
    {
        private List<TodoTask> Tasks = new List<TodoTask>();
        // List<TodoTask> is a collection that can hold multiple task objects.
        // Tasks is the name of that collection.
        // private means it's accessible inside MainPage

        private readonly HttpClient _httpClient = new HttpClient();
        private readonly WeatherService _weatherService = new WeatherService();

        private async Task LoadWeatherAsync()
        {
            try
            {
                CurrentWeather? weather = await _weatherService.GetWeatherAsync();

                if (weather != null)
                {
                    WeatherLabel.Text =
                        $"Temperature: {weather.Temperature:F1}°C | Wind: {weather.WindSpeed:F1} m/s";

                    if (weather.WindSpeed > 5)
                    {
                        WeatherLabel.Text += "\n🪁 Kitesurf weather!";
                    }
                }
                else
                {
                    WeatherLabel.Text = "Weather data unavailable";
                }
            }
            catch (HttpRequestException)
            {
                WeatherLabel.Text = "Unable to load weather. Check your internet connection.";
            }
            catch (TaskCanceledException)
            {
                WeatherLabel.Text = "Weather request timed out.";
            }
        }

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            double fontSize = Preferences.Default.Get("TaskFontSize", 16.0);

            await Shell.Current.GoToAsync(
                $"{nameof(SettingsPage)}?fontSize={fontSize}"
            );
        }

        private async Task LoadTasksAsync()
        {
            var tasks = await _httpClient.GetFromJsonAsync<List<TodoTask>>(
                "https://localhost:7187/api/tasks"
            );
            Tasks = tasks ?? new List<TodoTask>();
            DisplayTasks();
        }
        public MainPage()
        {


            InitializeComponent();

    }

        protected override async void OnAppearing()
        {

            base.OnAppearing();
            await LoadTasksAsync();
            await LoadWeatherAsync();
            string savedColor = Preferences.Default.Get("BackgroundColor", "White");

            BackgroundColor = savedColor switch
            {
                "Beige" => Color.FromArgb("#F5F5DC"),
                "Light Blue" => Color.FromArgb("#E3F2FD"),
                "Dark" => Color.FromArgb("#17191E"),
                "Vampire" => Color.FromArgb("#280D19"),
                _ => Colors.White
            };
            PageTitle.TextColor = savedColor switch
            {
                "Dark" => Colors.White,
                "Vampire" => Color.FromArgb("#FFB4C8"),
                _ => Colors.Black
            };

            Shell.SetBackgroundColor(this, savedColor switch
            {
                "Dark" => Color.FromArgb("#17191E"),
                "Vampire" => Color.FromArgb("#280D19"),
                "Beige" => Color.FromArgb("#F5F5DC"),
                "Light Blue" => Color.FromArgb("#E3F2FD"),
                _ => Colors.White
            });

            Shell.SetTitleColor(this, savedColor switch
            {
                "Dark" => Colors.White,
                "Vampire" => Color.FromArgb("#FFB4C8"),
                _ => Colors.Black
            });
        }

        private async void OnAddTaskClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TaskEntry.Text))
            {
                await DisplayAlertAsync("Empty task", "Please enter a task first", "OK");
                return;
            }
            TodoTask newTask = new TodoTask // Task object with title and completion status proerties
            {
                Title = TaskEntry.Text.Trim(),
                IsCompleted = false,
                CategoryId = 1,
                Priority = PriorityPicker.SelectedIndex + 1,
                DueDate = DueDatePicker.Date.HasValue
    ? DateTime.SpecifyKind(DueDatePicker.Date.Value, DateTimeKind.Utc)
    : null
            };
            var response = await _httpClient.PostAsJsonAsync(
     "https://localhost:7187/api/tasks",
     newTask
 );

            response.EnsureSuccessStatusCode();

            await LoadTasksAsync();
            TaskEntry.Text = "";

        }
        private void DisplayTasks()
        {

            var topTasks = Tasks
    .Where(task => !task.IsCompleted)
    .OrderByDescending(task => task.Priority)
    .ThenBy(task => task.DueDate ?? DateTime.MaxValue)
    .Take(3)
    .ToList();

            TopTasksList.Children.Clear();

            foreach (TodoTask task in topTasks)
            {
                string priorityName = task.Priority switch
                {
                    3 => "High",
                    2 => "Medium",
                    _ => "Low"
                };

                VerticalStackLayout taskLayout = new VerticalStackLayout
                {
                    Spacing = 4
                };

                Label titleLabel = new Label
                {
                    Text = task.Title,
                    FontSize = 16
                };

                Label priorityLabel = new Label
                {
                    Text = priorityName,
                    FontSize = 12
                };

                taskLayout.Children.Add(titleLabel);
                taskLayout.Children.Add(priorityLabel);

                TopTasksList.Children.Add(taskLayout);
            }

            TaskList.Children.Clear();
            foreach(TodoTask task in Tasks)
            {
                Border taskCard = new Border
                {
                    StrokeThickness = 1,
                    Stroke = Colors.LightGray,
                    StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle
                    {
                        CornerRadius = new CornerRadius(12)
                    },
                    Padding = new Thickness(12),
                    Margin = new Thickness(0, 4)
                };

                VerticalStackLayout cardContent = new VerticalStackLayout
                {
                    Spacing = 8
                };

                HorizontalStackLayout taskRow = new HorizontalStackLayout()
                {
                    Spacing = 10
                };

                HorizontalStackLayout tagsRow = new HorizontalStackLayout
                {
                    Spacing = 8
                };

                string priorityName = task.Priority switch
                {
                    3 => "High",
                    2 => "Medium",
                    _ => "Low"
                };

                Label priorityTag = new Label
                {
                    Text = priorityName,
                    FontSize = 12,
                    Padding = new Thickness(10, 5),
                    BackgroundColor = Color.FromArgb("#E8E8E8"),
                    TextColor = Colors.Black
                };

                tagsRow.Children.Add(priorityTag);

                Label dueDateTag = new Label
                {
                    Text = task.DueDate?.ToString("dd.MM.yyyy") ?? "No due date",
                    FontSize = 12,
                    Padding = new Thickness(10, 5),
                    BackgroundColor = Color.FromArgb("#E8E8E8"),
                    TextColor = Colors.Black
                };

                tagsRow.Children.Add(dueDateTag);

                Label categoryTag = new Label
                {
                    Text = task.CategoryId == 1 ? "General" : "Unknown",
                    FontSize = 12,
                    Padding = new Thickness(10, 5),
                    BackgroundColor = Color.FromArgb("#E8E8E8"),
                    TextColor = Colors.Black
                };

                tagsRow.Children.Add(categoryTag);

                Label taskLabel = new Label
                {
                    Text = task.Title,
                    VerticalOptions = LayoutOptions.Center,
                    TextDecorations = task.IsCompleted
    ? TextDecorations.Strikethrough
    : TextDecorations.None,
    FontSize = Preferences.Default.Get("TaskFontSize", 16),
                    TextColor = Preferences.Default.Get("BackgroundColor", "White") switch
                    {
                        "Dark" => Colors.White,
                        "Vampire" => Color.FromArgb("#FFB4C8"),
                        _ => Colors.Black
                    },
                };


                Button deleteButton = new Button
                {
                    Text = "Delete"
                };

                deleteButton.Clicked += async (sender, e) =>
                {
                    var response = await _httpClient.DeleteAsync(
                        $"https://localhost:7187/api/tasks/{task.Id}"
                    );

                    response.EnsureSuccessStatusCode();

                    await LoadTasksAsync();
                };

                Button editButton = new Button
                {
                    Text = "Edit"
                };


                editButton.Clicked += async (sender, e) =>
                {
                    await Shell.Current.GoToAsync(
                        $"{nameof(EditTaskPage)}?taskId={task.Id}"
                    );
                };


                CheckBox doneCheckBox = new CheckBox()
                {
                    IsChecked = task.IsCompleted

                };
                Label doneLabel = new Label
                {
                    Text = "Done",
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Preferences.Default.Get("BackgroundColor", "White") switch
                    {
                        "Dark" => Colors.White,
                        "Vampire" => Color.FromArgb("#FFB4C8"),
                        _ => Colors.Black
                    }
                };

                doneCheckBox.CheckedChanged += async (sender, e) =>
                {
                    task.IsCompleted = e.Value;

                    taskLabel.TextDecorations = task.IsCompleted ? TextDecorations.Strikethrough : TextDecorations.None;
                    var response = await _httpClient.PutAsJsonAsync(
    $"https://localhost:7187/api/tasks/{task.Id}",
    task
);

                    response.EnsureSuccessStatusCode();
                    await LoadTasksAsync();
                };




                taskRow.Children.Add(taskLabel);
              
                taskRow.Children.Add(editButton);
             
                taskRow.Children.Add(deleteButton);
                taskRow.Children.Add(doneCheckBox);
                taskRow.Children.Add(doneLabel);

                cardContent.Children.Add(taskRow);
                cardContent.Children.Add(tagsRow);
                taskCard.Content = cardContent;
                TaskList.Children.Add(taskCard);
            }
        }
    }
}
