using Microsoft.EntityFrameworkCore;
using ToDoApp.DataAccess.Entities;
using ToDoApp.Interfaces.DataAccess;

namespace ToDoApp.DataAccess.Repositories
{
    public class CategoryRepository : Repository<Category, int>, ICategoryRepository
    {
        private readonly ToDoDbContext _context;

        public CategoryRepository(ToDoDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetCategoriesByUserAsync(string userId)
        {
            return await _context.Categories
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAndUserAsync(int id, string userId)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
        }
    }
}
