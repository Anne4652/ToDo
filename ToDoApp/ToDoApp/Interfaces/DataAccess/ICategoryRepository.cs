using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.DataAccess.Entities;
using Task = ToDoApp.DataAccess.Entities.Task;

namespace ToDoApp.Interfaces.DataAccess
{
    public interface ICategoryRepository : IRepository<Category, int>
    {
        Task<IEnumerable<Category>> GetCategoriesByUserAsync(string userId);
        Task<Category> GetCategoryByIdAndUserAsync(int id, string userId);
    }
}
