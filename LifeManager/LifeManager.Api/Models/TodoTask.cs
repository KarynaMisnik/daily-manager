using System.ComponentModel.DataAnnotations;

namespace LifeManager.Api.Models;


public class TodoTask
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Task title is required.")] // [Required] tells ASP.NET Core that the title must have a value, rejects empty strings
    [StringLength(100, ErrorMessage = "Task title cannot exceed 100 characters.")]
    public string Title { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }

    public int CategoryId { get; set; }

    public Category? Category { get; set; }

    public int Priority { get; set; } = 1;

    public DateTime? DueDate { get; set; }
}