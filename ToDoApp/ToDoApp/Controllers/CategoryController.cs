using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.DataAccess.Entities;
using ToDoApp.Interfaces.Services;
using ToDoApp.Services.DTOs;

namespace ToDoApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("Test")]
        public async Task<IActionResult> GetCategoriesTest()
        {
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _categoryService.AddCategoryAsync(category);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryDto category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _categoryService.UpdateCategoryAsync(category);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var existingCategory = await _categoryService.GetCategoryByIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            await _categoryService.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}
