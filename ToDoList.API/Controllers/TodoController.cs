using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDoList.API.Contracts.Repositories;
using ToDoList.API.Controllers.DTOs;
using ToDoList.API.models;
using ToDoList.API.SetUnitOfWork;
using ToDoList.API.utils.mappers;
using ToDoList.API.utils.pagination;
using ToDoList.API.utils.Responses;

namespace ToDoList.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly IUnitOfWork _unit;

        public TodoController(IUnitOfWork unit)
        {
            _unit = unit;
        }

        [HttpGet]
        public async Task<ActionResult> GetPaginated(
            [FromQuery] int pageNumber = 1,  
            [FromQuery] int pageSize = 10,   
            [FromQuery] string? searchText = null) 
        {
            Expression<Func<TaskModel, bool>>? filter = null;

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                filter = task => task.Name.ToLower().Contains(searchText.ToLower()) ||
                                 task.Description.ToLower().Contains(searchText.ToLower());
            }

            long totalRecords = await _unit.TaskRepository.CountAsync(filter);
            List<TaskModel> tasks = await _unit.TaskRepository.GetPaginatedAsync(pageNumber, pageSize, filter);

            PagedResponse<TaskModel> pagedResponse = new PagedResponse<TaskModel>(tasks, pageNumber, pageSize, totalRecords);
            return Ok(pagedResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateTaskDTO task)
        {
            await _unit.TaskRepository.AddAsync(TaskMappear.CreateTaskDTOToTask(task));

            return Ok(new Response(
                "success",
                "Task created",
                201,
                ""
            ));
        }

        [HttpDelete("{Id:required}")]
        public async Task<IActionResult> Delete(string Id)
        {
            await _unit.TaskRepository.GetByIdAsync(Id);
            await _unit.TaskRepository.DeleteAsync(Id);

            return Ok(new Response(
                "success",
                "Task deleted",
                200,
                ""
            ));
        }

        [HttpGet("{Id:required}")]
        public async Task<IActionResult> Get(string Id)
        {
            TaskModel task = await _unit.TaskRepository.GetByIdAsync(Id);

            return Ok(new Response(
                "success",
                "Task founded",
                200,
                task
            ));
        }

        [HttpPut("{Id:required}")]
        public async Task<IActionResult> Update(string Id, [FromBody] UpdateTaskDTO dto )
        {
            TaskModel task = await _unit.TaskRepository.GetByIdAsync(Id);
            await _unit.TaskRepository.UpdateAsync(Id, TaskMappear.UpdateTaskDTOToTask(dto));

            return Ok(new Response(
                "success",
                "Task updated",
                200,
                ""
            ));
        }

    }
}