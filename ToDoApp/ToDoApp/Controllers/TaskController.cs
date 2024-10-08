using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApp.DataAccess.Entities;
using ToDoApp.Services;
using ToDoApp.Interfaces.Services;
using Task = ToDoApp.DataAccess.Entities.Task;
using ToDoApp.Services.DTOs;

namespace ToDoApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string searchTerm = "", [FromQuery] int? categoryId = null)
        {
            var tasks = await _taskService.GetTasksAsync(pageNumber, pageSize, searchTerm, categoryId);
            return Ok(tasks);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            var task = await _taskService.GetTaskByIdAndUserAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] CreateTaskDto task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _taskService.AddTaskAsync(task);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTask([FromBody] TaskDto task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _taskService.UpdateTaskAsync(task);
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var existingTask = await _taskService.GetTaskByIdAndUserAsync(id);
            if (existingTask == null)
            {
                return NotFound();
            }

            await _taskService.DeleteTaskAsync(id);
            return NoContent();
        }
    }
}
