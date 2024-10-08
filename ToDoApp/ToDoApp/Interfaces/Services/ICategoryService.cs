using System.Threading.Tasks;
using ToDoApp.DataAccess.Entities;
using ToDoApp.Services.DTOs;

namespace ToDoApp.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int id);
        System.Threading.Tasks.Task AddCategoryAsync(CategoryDto category);
        System.Threading.Tasks.Task UpdateCategoryAsync(CategoryDto category);
        System.Threading.Tasks.Task DeleteCategoryAsync(int id);
    }
}
