using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = ToDoApp.DataAccess.Entities.Task;

namespace ToDoApp.Interfaces.DataAccess
{
    public interface ITaskRepository : IRepository<Task, Guid>
    {
        Task<IEnumerable<Task>> GetTasksAsync(string userId, int pageNumber, int pageSize, string searchTerm, int? categoryId);
        Task<IEnumerable<Task>> GetTasksByUserAsync(string userId);
        Task<IEnumerable<Task>> GetTasksByCategoryAndUserAsync(int categoryId, string userId);
        Task<Task> GetTaskByIdAndUserAsync(Guid id, string userId);
    }
}
