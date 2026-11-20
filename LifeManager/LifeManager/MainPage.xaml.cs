
using System.Net.Http.Json;
using LifeManager.Models;
using LifeManager.Services;
using Microsoft.Maui.Controls.Shapes;


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
        private List<Category> Categories = new();

        private void OnCategoryFilterChanged(object sender, EventArgs e)
        {
            
            DisplayTasks();
        }

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

        private async void OnNewCategoryClicked(object sender, EventArgs e)
        {
            string categoryName = await DisplayPromptAsync(
                "New Category",
                "Enter a category name:",
                accept: "Create",
                cancel: "Cancel");

            if (string.IsNullOrWhiteSpace(categoryName))
            {
                return;
            }

            Category newCategory = new Category
            {
                Name = categoryName.Trim()
            };

            var response = await _httpClient.PostAsJsonAsync(
                "https://localhost:7187/api/categories",
                newCategory);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ApiValidationError>();

                var message = error?.Errors.Values
                    .SelectMany(messages => messages)
                    .FirstOrDefault() ?? "Category name is invalid.";

                await DisplayAlertAsync(
                    "Validation error",
                    message,
                    "OK"
                );

                return;
            }

            await LoadCategoriesAsync();
        }

        public MainPage()
        {


            InitializeComponent();

    }

        protected override async void OnAppearing()
        {

            base.OnAppearing();
            await LoadCategoriesAsync();
            await LoadTasksAsync();
            await LoadWeatherAsync();
            string savedColor = Preferences.Default.Get("BackgroundColor", "White");

            ThemeService.ApplyTheme(savedColor);


            DueDatePicker.SetDynamicResource(
    DatePicker.TextColorProperty,
    "ThemeText"
);

            SetDynamicResource(BackgroundColorProperty, "ThemeBackground");

            PageTitle.SetDynamicResource(
                Label.TextColorProperty,
                "ThemeText"
            );

        }

        private async Task LoadCategoriesAsync()
        {
            var categories = await _httpClient.GetFromJsonAsync<List<Category>>(
                "https://localhost:7187/api/categories");

            Categories = categories ?? new List<Category>();
            CategoryPicker.ItemsSource = Categories;
            CategoryPicker.SelectedIndex = 0;

            var filterCategories = new List<Category>
{
    new Category { Id = 0, Name = "All categories" }
};

            filterCategories.AddRange(Categories);

            CategoryFilterPicker.ItemsSource = filterCategories;
            CategoryFilterPicker.SelectedIndex = 0;
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
                CategoryId = ((Category)CategoryPicker.SelectedItem).Id,
                Priority = PriorityPicker.SelectedIndex + 1,
                DueDate = DueDatePicker.Date.HasValue
    ? DateTime.SpecifyKind(DueDatePicker.Date.Value, DateTimeKind.Utc)
    : null
            };
            var response = await _httpClient.PostAsJsonAsync(
     "https://localhost:7187/api/tasks",
     newTask
 );

            System.Diagnostics.Debug.WriteLine(
    $"POST status: {response.StatusCode}"
);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ApiValidationError>();

                var message = error?.Errors.Values
                    .SelectMany(messages => messages)
                    .FirstOrDefault() ?? "Task title is invalid.";

                await DisplayAlertAsync(
                    "Validation error",
                    message,
                    "OK"
                );

                return;
            }

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

                HorizontalStackLayout taskLayout = new HorizontalStackLayout
                {
                    Spacing = 12
                };

                Label titleLabel = new Label
                {
                    Text = task.Title,
                    FontSize = 16,
                    VerticalOptions = LayoutOptions.Center
                };

                Label priorityLabel = new Label
                {
                    Text = priorityName.ToUpper(),
                    FontSize = 12,
                    Padding = new Thickness(10, 5),
                    TextColor = Colors.Black,
                    VerticalOptions = LayoutOptions.Center
                };

                string priorityColor = task.Priority switch
                {
                    3 => "PriorityHigh",
                    2 => "PriorityMedium",
                    _ => "PriorityLow"
                };

                Border topPriorityBadge = new Border
                {
                    Content = priorityLabel,
                    StrokeThickness = 0,
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = new CornerRadius(8)
                    }
                };

                topPriorityBadge.SetDynamicResource(
                    Border.BackgroundColorProperty,
                    priorityColor
                );
                taskLayout.Children.Add(titleLabel);
                taskLayout.Children.Add(topPriorityBadge);

                TopTasksList.Children.Add(taskLayout);
            }

            var filteredTasks = Tasks.Where(task =>
      CategoryFilterPicker.SelectedItem is not Category selectedCategory
      || selectedCategory.Id == 0
      || task.CategoryId == selectedCategory.Id
  ).ToList();

            TaskList.ItemTemplate = new DataTemplate(() =>
            {
                var container = new ContentView();

                container.BindingContextChanged += (sender, e) =>
                {
                    if (container.BindingContext is TodoTask task)
                    {
                        container.Content = CreateTaskCard(task);
                    }
                };

                return container;
            });

            if (SortPicker.SelectedIndex == 0) // Priority
            {
                filteredTasks = filteredTasks
                    .OrderByDescending(task => task.Priority)
                    .ToList();
            }
            else if (SortPicker.SelectedIndex == 1) // Due date
            {
                filteredTasks = filteredTasks
                    .OrderBy(task => task.DueDate ?? DateTime.MaxValue)
                    .ToList();
            }

            TaskList.ItemsSource = filteredTasks;
        }

        private Border CreateTaskCard(TodoTask task)
        {
            Border taskCard = new Border
            {
                StrokeThickness = 0.5,
                StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle
                {
                    CornerRadius = new CornerRadius(12)
                },
                Padding = new Thickness(12),
                Margin = new Thickness(0, 4)
            };

            taskCard.SetDynamicResource(
                Border.StrokeProperty,
                "ThemeBorder"
            );

            taskCard.SetDynamicResource(
                Border.BackgroundColorProperty,
                "ThemeSurface"
            );

            VerticalStackLayout cardContent = new VerticalStackLayout
            {
                Spacing = 8
            };

            Grid taskRow = new Grid
            {
                ColumnDefinitions =
    {
       new ColumnDefinition(GridLength.Auto), // Title
        new ColumnDefinition(GridLength.Auto), // Checkbox
        new ColumnDefinition(GridLength.Star), // Empty flexible space
        new ColumnDefinition(GridLength.Auto), // Edit
        new ColumnDefinition(GridLength.Auto)  // Delete
    },
                ColumnSpacing = 8
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
                Text = priorityName.ToUpper(),
                FontSize = 12,
                Padding = new Thickness(10, 5),
                TextColor = Colors.Black,

            };

            string priorityColor = task.Priority switch
            {
                3 => "PriorityHigh",
                2 => "PriorityMedium",
                _ => "PriorityLow"
            };

            Border priorityBadge = new Border
            {
                Content = priorityTag,
                StrokeThickness = 0,
                StrokeShape = new RoundRectangle
                {
                    CornerRadius = new CornerRadius(8)
                }
            };

            priorityBadge.SetDynamicResource(
                Border.BackgroundColorProperty,
                priorityColor
            );

            tagsRow.Children.Add(priorityBadge);

            Label categoryTag = new Label
            {
                Text = Categories.FirstOrDefault(c => c.Id == task.CategoryId)?.Name
   ?? "Unknown",
                FontSize = 12,
                Padding = new Thickness(10, 5),
                TextColor = Colors.Black
            };

            // Select a consistent color based on CategoryId
            int colorIndex = task.CategoryId % 8; // calculates the remainder after division

            Border categoryBadge = new Border
            {
                Content = categoryTag,
                StrokeThickness = 0,
                StrokeShape = new RoundRectangle
                {
                    CornerRadius = new CornerRadius(8)
                }
            };

            categoryBadge.SetDynamicResource(
                Border.BackgroundColorProperty,
                $"CategoryColor{colorIndex}"
            );

            tagsRow.Children.Add(categoryBadge);

            Label dueDateTag = new Label
            {
                Text = $"◷ {task.DueDate?.ToString("dd.MM.yyyy") ?? "No due date"}",
                FontSize = 12,
                Padding = new Thickness(10, 5),
                VerticalTextAlignment = TextAlignment.Center
            };

            dueDateTag.SetDynamicResource(
                Label.TextColorProperty,
                "ThemeText"
            );

            Border dueDateBadge = new Border
            {
                Content = dueDateTag,
                StrokeThickness = 1,
                StrokeShape = new RoundRectangle
                {
                    CornerRadius = new CornerRadius(8)
                }
            };

            dueDateBadge.SetDynamicResource(
                Border.BackgroundColorProperty,
                "ThemeSurface"
            );

            dueDateBadge.SetDynamicResource(
                Border.StrokeProperty,
                "ThemeBorder"
            );

            tagsRow.Children.Add(dueDateBadge);



            Label taskLabel = new Label
            {
                Text = task.Title,
                VerticalOptions = LayoutOptions.Center,
                TextDecorations = task.IsCompleted
       ? TextDecorations.Strikethrough
       : TextDecorations.None,
                FontSize = Preferences.Default.Get("TaskFontSize", 16)
            };

            taskLabel.SetDynamicResource(
                Label.TextColorProperty,
                "ThemeText"
            );


            Button deleteButton = new Button
            {
                Text = "🗑",
                FontSize = 20,
                Padding = new Thickness(4),
                WidthRequest = 32,
                HeightRequest = 32,
                BackgroundColor = Colors.Transparent,
                BorderWidth = 0
            };

            deleteButton.SetDynamicResource(
                Button.TextColorProperty, "ThemeText");

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
                Text = "✎",
                FontSize = 20,
                Padding = new Thickness(4),
                WidthRequest = 32,
                HeightRequest = 32,
                BackgroundColor = Colors.Transparent,
                BorderWidth = 0
            };

            editButton.SetDynamicResource(
                Button.TextColorProperty, "ThemeText");


            editButton.Clicked += async (sender, e) =>
            {
                await Shell.Current.GoToAsync(
                    $"{nameof(EditTaskPage)}?taskId={task.Id}"
                );
            };


            CheckBox doneCheckBox = new CheckBox()
            {
                IsChecked = task.IsCompleted,
                Margin = new Thickness(0, 0, -20, 0)

            };
            Label doneLabel = new Label
            {
                Text = "Done",
                VerticalOptions = LayoutOptions.Center
            };

            doneLabel.SetDynamicResource(
                Label.TextColorProperty,
                "ThemeText"
            );

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




            taskRow.Add(taskLabel, 0, 0);
            taskRow.Add(doneCheckBox, 1, 0);
            taskRow.Add(editButton, 3, 0);
            taskRow.Add(deleteButton, 4, 0);

            cardContent.Children.Add(taskRow);
            cardContent.Children.Add(tagsRow);
            taskCard.Content = cardContent;

            return taskCard;
        }

        // sorting tasks by due date or priority

        private void OnSortChanged(object sender, EventArgs e)
        {
            DisplayTasks();
        }
    }

   

    public class ApiValidationError
    {
        public Dictionary<string, string[]> Errors { get; set; } = new();
    }
}
