using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
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
        [EnableRateLimiting("SlidingWindowLimiterPolicy")]
        public async Task<ActionResult> GetPaginated([FromQuery] TaskQueryStruct taskQuery) {
            Expression<Func<TaskModel, bool>>? filter = null;

            filter = TaskFilter.ApplyFilter(taskQuery, filter);

            long totalRecords = await _unit.TaskRepository.CountAsync(filter);
            List<TaskModel> tasks = await _unit.TaskRepository.GetPaginatedAsync(taskQuery.PageNumber, taskQuery.PageSize, filter);

            PagedResponse<TaskModel> pagedResponse = new PagedResponse<TaskModel>(tasks, taskQuery.PageNumber, taskQuery.PageSize, totalRecords);
            return Ok(pagedResponse);
        }

        [HttpPost]
        [EnableRateLimiting("SlidingWindowLimiterPolicy")]
        public async Task<IActionResult> Add([FromBody] CreateTaskDTO task)
        {
            TaskModel taskCreated = await _unit.TaskRepository.AddAsync(TaskMappear.CreateTaskDTOToTask(task));
             

            return Ok(new Response<TaskModel>(
                "success",
                "Task created",
                201,
                taskCreated
            ));
        }

        [HttpDelete("{Id:required}")]
        [EnableRateLimiting("SlidingWindowLimiterPolicy")]
        public async Task<IActionResult> Delete(string Id)
        {
            await _unit.TaskRepository.GetByIdAsync(Id);
            await _unit.TaskRepository.DeleteAsync(Id);

            return Ok(new Response<string>(
                "success",
                "Task deleted",
                200,
                ""
            ));
        }

        [HttpGet("{Id:required}")]
        [EnableRateLimiting("SlidingWindowLimiterPolicy")]
        public async Task<IActionResult> Get(string Id)
        {
            TaskModel task = await _unit.TaskRepository.GetByIdAsync(Id);

            return Ok(new Response<TaskModel>(
                "success",
                "Task founded",
                200,
                task
            ));
        }

        [HttpPut("{Id:required}")]
        [EnableRateLimiting("SlidingWindowLimiterPolicy")]
        public async Task<IActionResult> Update(string Id, [FromBody] UpdateTaskDTO dto )
        {
            TaskModel task = await _unit.TaskRepository.GetByIdAsync(Id);
            await _unit.TaskRepository.UpdateAsync(Id, TaskMappear.UpdateTaskDTOToTask(dto));

            return Ok(new Response<string>(
                "success",
                "Task updated",
                200,
                ""
            ));
        }

        [HttpDelete("delete-many")]
        [EnableRateLimiting("SlidingWindowLimiterPolicy")]
        public async Task<IActionResult> DeleteMany(List<string> Ids) 
        {
            await this._unit.TaskRepository.DeleteManyAsync(Ids);
            await this._unit.CompleteAsync();

            return Ok(new Response<string>(
                "success",
                "Tasks deleted!",
                200,
                ""
            ));
        }

        [HttpGet("change/{Id:required}")]
        [EnableRateLimiting("SlidingWindowLimiterPolicy")]
        public async Task<IActionResult> ChangeStatus(string Id)
        {
            TaskModel task = await _unit.TaskRepository.GetByIdAsync(Id);
            await _unit.TaskRepository.ChangeComepleteTask(Id, task);
            task.IsComplete = !task.IsComplete;

            return Ok(new Response<TaskModel>(
                "success",
                "Task changed",
                200,
                task
            ));
        }

    }
}