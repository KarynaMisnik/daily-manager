using LifeManager.Models;

namespace LifeManager.Interfaces
{
    public interface ITodoService
    {
        Task<List<TodoTask>> GetTasksAsync();
        Task<HttpResponseMessage> CreateTaskAsync(TodoTask task);
        Task DeleteTaskAsync(int id);
        Task UpdateTaskAsync(TodoTask task);

        Task<TodoTask?> GetTaskByIdAsync(int id);
    }
}