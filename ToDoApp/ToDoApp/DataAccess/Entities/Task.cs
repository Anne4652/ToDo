using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToDoApp.DataAccess.Entities;

namespace ToDoApp.DataAccess.Entities
{
    public class Task
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public bool IsCompleted { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public string UserId { get; set; }

        public IdentityUser User { get; set; }
    }
}
