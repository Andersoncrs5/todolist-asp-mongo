using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult> GetPaginated([FromQuery] TaskQueryStruct taskQuery) {
            Expression<Func<TaskModel, bool>>? filter = null;

            filter = TaskFilter.ApplyFilter(taskQuery, filter);

            long totalRecords = await _unit.TaskRepository.CountAsync(filter);
            List<TaskModel> tasks = await _unit.TaskRepository.GetPaginatedAsync(taskQuery.PageNumber, taskQuery.PageSize, filter);

            PagedResponse<TaskModel> pagedResponse = new PagedResponse<TaskModel>(tasks, taskQuery.PageNumber, taskQuery.PageSize, totalRecords);
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

        [HttpDelete("delete-many")]
        public async Task<IActionResult> DeleteMany(List<string> Ids) 
        {
            await this._unit.TaskRepository.DeleteManyAsync(Ids);
            await this._unit.CompleteAsync();

            return Ok(new Response(
                "success",
                "Tasks deleted!",
                200,
                ""
            ));
        }

        [HttpPatch("{Id:required}")]
        public async Task<IActionResult> ChangeStatus(string Id)
        {
            TaskModel task = await _unit.TaskRepository.GetByIdAsync(Id);
            await _unit.TaskRepository.ChangeComepleteTask(Id, task);

            return Ok(new Response(
                "success",
                "Task changed",
                200,
                task
            ));
        }

        
    }
}