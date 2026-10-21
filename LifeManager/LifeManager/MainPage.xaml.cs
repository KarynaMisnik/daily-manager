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
            string task = TaskEntry.Text;
            HorizontalStackLayout taskRow = new HorizontalStackLayout();

            Label taskLabel = new Label
            {
                Text = task,
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

                taskLabel.TextDecorations = e.Value ? TextDecorations.Strikethrough : TextDecorations.None;
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
