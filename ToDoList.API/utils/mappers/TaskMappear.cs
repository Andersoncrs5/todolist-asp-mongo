using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.API.Controllers.DTOs;
using ToDoList.API.models;

namespace ToDoList.API.utils.mappers
{
    public class TaskMappear
    {
        public TaskModel CreateTaskDTOToTask(CreateTaskDTO dto) 
        {
            return new TaskModel
            { 
                Name = dto.Name,
                Description = dto.Description,
                IsComplete = dto.IsComplete,
            };
        }

        public CreateTaskDTO TaskToCreateTaskDTO(TaskModel task) 
        {
            return new CreateTaskDTO
            { 
                Name = task.Name,
                Description = task.Description,
                IsComplete = task.IsComplete,
            };
        }

    }
}