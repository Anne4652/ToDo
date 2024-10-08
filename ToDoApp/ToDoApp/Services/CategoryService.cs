using AutoMapper;
using System.Threading.Tasks;
using ToDoApp.DataAccess.Entities;
using ToDoApp.Interfaces.DataAccess;
using ToDoApp.Interfaces.Services;
using ToDoApp.Services.DTOs;

namespace ToDoApp.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CategoryService> _logger;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository,
            ILogger<CategoryService> logger, 
            IUserContextService userContextService,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
            _userContextService = userContextService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            var userId = _userContextService.GetUserId();
            _logger.LogInformation("Fetching all categories for user {UserId}", userId);

            return await _categoryRepository.GetCategoriesByUserAsync(userId);
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var userId = _userContextService.GetUserId();
            _logger.LogInformation("Fetching category {CategoryId} for user {UserId}", id, userId);

            return await _categoryRepository.GetCategoryByIdAndUserAsync(id, userId);
        }

        public async System.Threading.Tasks.Task AddCategoryAsync(CategoryDto category)
        {
            category.UserId = _userContextService.GetUserId();

            _logger.LogInformation("Adding a new category for user {UserId}, Name: {CategoryName}", category.UserId, category.Name);

            await _categoryRepository.Create(_mapper.Map<Category>(category));
        }

        public async System.Threading.Tasks.Task UpdateCategoryAsync(CategoryDto category)
        {
            var userId = _userContextService.GetUserId();
            var existingCategory = await _categoryRepository.GetCategoryByIdAndUserAsync(category.Id, userId);
            if (existingCategory == null)
            {
                throw new ArgumentException("No such category");
            }
            existingCategory.Name = category.Name;
            _logger.LogInformation("Updating category {CategoryId} for user {UserId}, Name: {CategoryName}", category.Id, category.UserId, category.Name);

            await _categoryRepository.Update(existingCategory);
        }

        public async System.Threading.Tasks.Task DeleteCategoryAsync(int id)
        {
            var userId = _userContextService.GetUserId();
            _logger.LogInformation("Deleting category {CategoryId} for user {UserId}", id, userId);

            var category = await _categoryRepository.GetCategoryByIdAndUserAsync(id, userId);

            if (category != null)
            {
                await _categoryRepository.Delete(id);
                _logger.LogInformation("Category {CategoryId} deleted successfully", id);
            }
            else
            {
                _logger.LogWarning("Attempted to delete category {CategoryId}, but it was not found for user {UserId}", id, userId);
                throw new KeyNotFoundException($"Category with ID {id} not found.");
            }
        }
    }
}
