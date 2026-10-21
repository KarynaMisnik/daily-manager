# Life Manager

**MainPage.xaml** — defines the UI layout of your page.
<br>
**AppShell.xaml** — defines the app's navigation structure.
<br>
**MainPage.xaml.cs and AppShell.xaml.cs** — contain the C# logic for each respective page.

**MainPage** — the class representing your page.<br>
**public MainPage()** — the constructor, which runs when the page is created.<br>
**InitializeComponent()** — loads the UI defined in MainPage.xaml.<br>
**ContentPage** — the MAUI page type your class inherits from.<br>

### MainPage.xaml Elements

**Entry** — a text input, similar to React Native's TextInput.<br>
**x:Name="TaskEntry"** — gives this input a name so C# can access it later.<br>
**Placeholder** — the hint displayed when the field is empty.<br>

**OnButtonClicked** — the method called when the button is clicked.<br>
The flow:<br>
[-] The user clicks the button.
[-] MAUI sees Clicked="OnAddTaskClicked" in XAML.
[-] MAUI calls this C# method.

```c#
 private void OnAddTaskClicked(object sender, EventArgs e)
        {
        }
```
<br>
The parameters sender and e provide information about the event.
<br>

```c#
string task = TaskEntry.Text;
```

**TaskEntry** refers to the input we named in XAML.<br>
```.Text``` gets whatever the user typed.<br>
**string task** stores that text in a C# variable.<br>
```c#

```c#
 TaskList.Children.Add(taskLabel);
```

This means: take the Label we just created and put it inside TaskList.

```c#
 private void OnAddTaskClicked(object sender, EventArgs e)
```
changed into (look at DisplayAlertAsync)

```c#
private async void OnAddTaskClicked(object sender, EventArgs e)
```

**await** lets the app wait for the alert to finish before continuing. The **return** still stops the method, so an empty task won't be added.
