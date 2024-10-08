using Microsoft.EntityFrameworkCore;
using ToDoApp.Interfaces.DataAccess;
using Task = ToDoApp.DataAccess.Entities.Task;

namespace ToDoApp.DataAccess.Repositories
{
    public class TaskRepository : Repository<Task, Guid>, ITaskRepository
    {
        private readonly ToDoDbContext _context;

        public TaskRepository(ToDoDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Task>> GetTasksAsync(string userId, int pageNumber, int pageSize, string searchTerm, int? categoryId)
        {
            var query = _context.Tasks
                .Where(t => t.UserId == userId)
                .Include(t => t.Category)
                .AsQueryable();

            if (categoryId.HasValue)
            {
                query = query.Where(t => t.CategoryId == categoryId);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(t => t.Title.Contains(searchTerm) || t.Description.Contains(searchTerm));
            }

            var tasks = await query
                .OrderBy(t => t.DueDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return tasks;
        }


        public async Task<IEnumerable<Task>> GetTasksByUserAsync(string userId)
        {
            return await _context.Tasks
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Task>> GetTasksByCategoryAndUserAsync(int categoryId, string userId)
        {
            return await _context.Tasks
                .Where(t => t.CategoryId == categoryId && t.UserId == userId)
                .ToListAsync();
        }
        public async Task<Task> GetTaskByIdAndUserAsync(Guid id, string userId)
        {
            return await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        }
    }
}
