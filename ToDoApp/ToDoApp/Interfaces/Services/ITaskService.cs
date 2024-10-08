using ToDoApp.Services.DTOs;
using Task = ToDoApp.DataAccess.Entities.Task;

namespace ToDoApp.Interfaces.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDto>> GetTasksAsync(int pageNumber, int pageSize, string searchTerm, int? categoryId);
        Task<TaskDto> GetTaskByIdAndUserAsync(Guid id);
        System.Threading.Tasks.Task AddTaskAsync(CreateTaskDto task);
        System.Threading.Tasks.Task UpdateTaskAsync(TaskDto task);
        System.Threading.Tasks.Task DeleteTaskAsync(Guid id);
    }
}
