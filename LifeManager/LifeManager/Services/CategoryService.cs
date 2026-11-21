using System.Net.Http.Json;
using LifeManager.Interfaces;
using LifeManager.Models;

namespace LifeManager.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> CreateCategoryAsync(Category category)
        {
            return await _httpClient.PostAsJsonAsync(
                "https://localhost:7187/api/categories",
                category
            );
        }
        public async Task<List<Category>> GetCategoriesAsync()
        {
            var categories = await _httpClient.GetFromJsonAsync<List<Category>>(
                "https://localhost:7187/api/categories"
            );

            return categories ?? new List<Category>();
        }
    }
}