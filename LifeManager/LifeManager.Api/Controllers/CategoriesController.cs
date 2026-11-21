using LifeManager.Api.Data;
using LifeManager.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace LifeManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        return await _context.Categories.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Category>> CreateCategory(Category category)
    {
        if (string.IsNullOrWhiteSpace(category.Name))
        {
            return BadRequest("Category name cannot be empty.");
        }

        category.Name = category.Name.Trim();
        bool categoryExists = await _context.Categories
    .AnyAsync(c => c.Name.ToLower() == category.Name.ToLower());

        if (categoryExists)
        {
            return BadRequest(new
            {
                errors = new Dictionary<string, string[]>
                {
                    ["Name"] = new[] { "Category already exists." }
                }
            });
        }

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetCategories),
            new { id = category.Id },
            category);
    }
}