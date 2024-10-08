using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Services.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? UserId { get; set; }
    }
}
