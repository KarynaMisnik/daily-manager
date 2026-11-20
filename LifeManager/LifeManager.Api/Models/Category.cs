using System.ComponentModel.DataAnnotations;

namespace LifeManager.Api.Models;


public class Category
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Category name is required.")]
    [StringLength(20, ErrorMessage = "Category name cannot exceed 20 characters.")]
    public string Name { get; set; } = string.Empty;

    public List<TodoTask> Tasks { get; set; } = new();
}