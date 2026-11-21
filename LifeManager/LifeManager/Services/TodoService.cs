using System.Net.Http.Json;
using LifeManager.Interfaces;
using LifeManager.Models;

namespace LifeManager.Services
{
    public class TodoService : ITodoService
    {
        private readonly HttpClient _httpClient;

        public TodoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TodoTask>> GetTasksAsync()
        {
            var tasks = await _httpClient.GetFromJsonAsync<List<TodoTask>>(
                "https://localhost:7187/api/tasks"
            );

            return tasks ?? new List<TodoTask>();
        }
        public async Task<HttpResponseMessage> CreateTaskAsync(TodoTask task)
        {
            return await _httpClient.PostAsJsonAsync(
                "https://localhost:7187/api/tasks",
                task
            );
        }
        public async Task DeleteTaskAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(
                $"https://localhost:7187/api/tasks/{id}"
            );

            response.EnsureSuccessStatusCode();
        }
        public async Task UpdateTaskAsync(TodoTask task)
        {
            var response = await _httpClient.PutAsJsonAsync(
                $"https://localhost:7187/api/tasks/{task.Id}",
                task
            );

            response.EnsureSuccessStatusCode();
        }
        public async Task<TodoTask?> GetTaskByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<TodoTask>(
                $"https://localhost:7187/api/tasks/{id}"
            );
        }
    }
}