using LifeManager.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<TodoTask> Tasks { get; set; }
    public DbSet<Category> Categories { get; set; }

}