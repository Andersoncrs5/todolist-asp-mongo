using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDoList.API.Contracts.Repositories;
using ToDoList.API.models;
using ToDoList.API.SetUnitOfWork;

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

        [HttpGet("/get-all")]
        public async Task<IActionResult> GetAll() 
        {
            var result = await _unit.TaskRepository.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] TaskModel task)
        {
            
        }

    }
}