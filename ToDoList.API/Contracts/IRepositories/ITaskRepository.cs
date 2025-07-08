using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.API.models;

namespace ToDoList.API.Contracts.IRepositories
{
    public interface ITaskRepository
    {
        Task<List<TaskModel>> GetAllAsync();
        Task<TaskModel> GetByIdAsync(string id);
        Task AddAsync(TaskModel item);
        Task UpdateAsync(TaskModel exists, TaskModel item);
        Task DeleteAsync(string id);

    }
}