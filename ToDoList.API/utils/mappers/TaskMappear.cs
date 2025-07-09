using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.API.Controllers.DTOs;
using ToDoList.API.models;
using MongoDB.Bson;

namespace ToDoList.API.utils.mappers
{
    public class TaskMappear
    {
        public static TaskModel CreateTaskDTOToTask(CreateTaskDTO dto) 
        {
            return new TaskModel
            { 
                Id = ObjectId.GenerateNewId().ToString(),
                Name = dto.Name,
                Description = dto.Description,
                IsComplete = dto.IsComplete,
            };
        }

        public static CreateTaskDTO TaskToCreateTaskDTO(TaskModel task) 
        {
            return new CreateTaskDTO
            { 
                Name = task.Name,
                Description = task.Description,
                IsComplete = task.IsComplete,
            };
        }

        public static TaskModel UpdateTaskDTOToTask(UpdateTaskDTO dto) 
        {
            return new TaskModel
            { 
                Name = dto.Name,
                Description = dto.Description,
                IsComplete = dto.IsComplete,
            };
        }

        public static UpdateTaskDTO TaskToUpdateTaskDTO(TaskModel task) 
        {
            return new UpdateTaskDTO
            { 
                Name = task.Name,
                Description = task.Description,
                IsComplete = task.IsComplete,
            };
        }

    }
}