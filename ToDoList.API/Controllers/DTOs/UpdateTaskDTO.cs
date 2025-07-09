using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.API.Controllers.DTOs
{
    public class UpdateTaskDTO
    {
        [Required(ErrorMessage = "Task name is required.")] 
        [StringLength(50, MinimumLength = 5, ErrorMessage = "The max is 50 and min is 5")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "The max is 200")]
        public string Description { get; set; } = string.Empty;
        public bool IsComplete { get; set; } = false;
    }
}