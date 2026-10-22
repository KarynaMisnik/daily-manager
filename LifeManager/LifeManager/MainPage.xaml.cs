namespace LifeManager
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
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
                IsCompleted = false
            };
            HorizontalStackLayout taskRow = new HorizontalStackLayout()
            {
                Spacing = 10
            };

            Label taskLabel = new Label
            {
                Text = newTask.Title,
                VerticalOptions = LayoutOptions.Center
            };


            CheckBox checkBox = new CheckBox();
            CheckBox doneCheckBox = new CheckBox();
            Label doneLabel = new Label
            {
                Text = "Done",
                VerticalOptions = LayoutOptions.Center
            };
            {

            }
            ;
            doneCheckBox.CheckedChanged += (sender, e) =>
            {
                newTask.IsCompleted = e.Value;

                taskLabel.TextDecorations = newTask.IsCompleted ? TextDecorations.Strikethrough : TextDecorations.None;
            };

            {


                taskRow.Children.Add(checkBox);
                taskRow.Children.Add(taskLabel);
                taskRow.Children.Add(doneCheckBox);
                taskRow.Children.Add(doneLabel);

                TaskList.Children.Add(taskRow);
                TaskEntry.Text = string.Empty;
            }

        }
    }
}
