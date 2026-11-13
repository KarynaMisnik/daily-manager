namespace LifeManager.Api.Models;

public class TodoTask
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }

    public int CategoryId { get; set; }

    public Category? Category { get; set; }

    public int Priority { get; set; } = 1;

    public DateTime? DueDate { get; set; }
}