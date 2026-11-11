
using System.Net.Http.Json;

namespace LifeManager
{
    public partial class MainPage : ContentPage
    {
        private List<TodoTask> Tasks = new List<TodoTask>();
        // List<TodoTask> is a collection that can hold multiple task objects.
        // Tasks is the name of that collection.
        // private means it's accessible inside MainPage

        private readonly HttpClient _httpClient = new HttpClient();

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
                Title = TaskEntry.Text,
                IsCompleted = false,
                CategoryId = 1
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
            TaskList.Children.Clear();
            foreach(TodoTask task in Tasks)
            {
                HorizontalStackLayout taskRow = new HorizontalStackLayout()
                {
                    Spacing = 10
                };
                Label taskLabel = new Label
                {
                    Text = task.Title,
                    VerticalOptions = LayoutOptions.Center,
                    TextDecorations = task.IsCompleted
    ? TextDecorations.Strikethrough
    : TextDecorations.None
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

                CheckBox doneCheckBox = new CheckBox()
                {
                    IsChecked = task.IsCompleted

                };
                Label doneLabel = new Label
                {
                    Text = "Done",
                    VerticalOptions = LayoutOptions.Center
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
                };


                taskRow.Children.Add(deleteButton);
                taskRow.Children.Add(taskLabel);
                taskRow.Children.Add(doneCheckBox);
                taskRow.Children.Add(doneLabel);

                TaskList.Children.Add(taskRow);
            }
        }
    }
}
