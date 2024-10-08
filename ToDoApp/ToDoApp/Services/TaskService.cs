using AutoMapper;
using ToDoApp.Interfaces.DataAccess;
using ToDoApp.Interfaces.Services;
using ToDoApp.Services.DTOs;
using Task = ToDoApp.DataAccess.Entities.Task;

namespace ToDoApp.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<TaskService> _logger;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;

        public TaskService(ITaskRepository taskRepository, ILogger<TaskService> logger, IUserContextService userContextService, IMapper mapper, ICategoryRepository categoryRepository)
        {
            _taskRepository = taskRepository;
            _logger = logger;
            _userContextService = userContextService;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<TaskDto>> GetTasksAsync(int pageNumber, int pageSize, string searchTerm, int? categoryId)
        {
            var userId = _userContextService.GetUserId();

            _logger.LogInformation("Getting tasks for user {UserId}, PageNumber: {PageNumber}, PageSize: {PageSize}", userId, pageNumber, pageSize);
            var tasks = await _taskRepository.GetTasksAsync(userId, pageNumber, pageSize, searchTerm, categoryId);
            return _mapper.Map<IEnumerable<TaskDto>>(tasks);
        }

        public async Task<TaskDto> GetTaskByIdAndUserAsync(Guid id)
        {
            var userId = _userContextService.GetUserId();

            _logger.LogInformation("Getting task {GetTaskByIdAndUserAsyncTaskId} for user {UserId}", id, userId);
            var task = await _taskRepository.GetTaskByIdAndUserAsync(id, userId);

            if (task == null)
            {
                _logger.LogWarning("Task {TaskId} for user {UserId} not found", id, userId);
            }
            return _mapper.Map<TaskDto>(task);
        }

        public async System.Threading.Tasks.Task AddTaskAsync(CreateTaskDto taskDto)
        {
            var userId = _userContextService.GetUserId();
            _logger.LogInformation("Adding a new task for user {UserId}, Title: {Title}", userId, taskDto.Title);
            var task = _mapper.Map<Task>(taskDto);
            task.UserId = userId;
            await _taskRepository.Create(task);
        }

        public async System.Threading.Tasks.Task UpdateTaskAsync(TaskDto taskDto)
        {
            var userId = _userContextService.GetUserId();
            var existingTask = await _taskRepository.GetTaskByIdAndUserAsync(taskDto.Id, userId);
            if (existingTask == null)
            {
                throw new ArgumentException("Such task does not exist");
            }

            _logger.LogInformation("Updating task {TaskId} for user {UserId}, Title: {Title}", taskDto.Id, userId, taskDto.Title);

            existingTask.Title = taskDto.Title;
            existingTask.Description = taskDto.Description;
            existingTask.DueDate = taskDto.DueDate.ToUniversalTime();
            existingTask.IsCompleted = taskDto.IsCompleted;

            var category = await _categoryRepository.GetCategoryByIdAndUserAsync(taskDto.Category.Id, userId);
            if (category == null)
            {
                throw new ArgumentException("Category does not exist");
            }
            existingTask.CategoryId = category.Id;
            existingTask.Category = category;

            await _taskRepository.Update(existingTask);
        }

        public async System.Threading.Tasks.Task DeleteTaskAsync(Guid id)
        {
            var userId = _userContextService.GetUserId();

            _logger.LogInformation("Deleting task {TaskId} for user {UserId}", id, userId);
            var task = await _taskRepository.GetTaskByIdAndUserAsync(id, userId);

            if (task != null)
            {
                await _taskRepository.Delete(id);
                _logger.LogInformation("Task {TaskId} deleted successfully", id);
            }
            else
            {
                _logger.LogWarning("Attempt to delete task {TaskId} for user {UserId}, but task was not found", id, userId);
            }
        }
    }
}
