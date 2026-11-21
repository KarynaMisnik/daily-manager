using LifeManager.Models;

namespace LifeManager.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategoriesAsync();
        Task<HttpResponseMessage> CreateCategoryAsync(Category category);

    }
}