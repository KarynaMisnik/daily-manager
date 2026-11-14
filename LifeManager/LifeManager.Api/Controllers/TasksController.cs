using LifeManager.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LifeManager.Api.Models;

namespace LifeManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;

    public TasksController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks() // retrieves all tasks from PSQL, returns in JSON
    {
        var tasks = await _context.Tasks.ToListAsync();
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id);

        if (task == null)
        {
            return NotFound();
        }

        return Ok(task);
    }

    [HttpPost] // tells ASP.NET Core to handle POST requests to /api/tasks
    public async Task<IActionResult> CreateTask(TodoTask task)
    {
        _context.Tasks.Add(task); // tells EF Core to track the new task for insertion
        await _context.SaveChangesAsync();

        return Ok(task); //returns the saved task, including its generated ID
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, TodoTask updatedTask)
    {
        var task = await _context.Tasks.FindAsync(id); // searches PostgreSQL for the task

        if (task == null)
        {
            return NotFound();
        }

        task.Title = updatedTask.Title;
        task.IsCompleted = updatedTask.IsCompleted;
        task.CategoryId = updatedTask.CategoryId;
        task.Priority = updatedTask.Priority;
        task.DueDate = updatedTask.DueDate;

        await _context.SaveChangesAsync();

        return Ok(task);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id);

        if (task == null)
        {
            return NotFound();
        }

        _context.Tasks.Remove(task); // marks the task for deletion
        await _context.SaveChangesAsync();

        return NoContent(); // returns HTTP 204, meaning the operation succeeded without returning data
    }
}