namespace LifeManager.Api.Models;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<TodoTask> Tasks { get; set; } = new();
}