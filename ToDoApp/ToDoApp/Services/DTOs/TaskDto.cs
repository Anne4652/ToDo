using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using ToDoApp.DataAccess.Entities;

namespace ToDoApp.Services.DTOs
{
    public class TaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public CategoryDto Category { get; set; }
    }
}
