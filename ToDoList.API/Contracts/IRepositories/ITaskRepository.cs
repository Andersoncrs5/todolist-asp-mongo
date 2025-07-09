using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ToDoList.API.models;

namespace ToDoList.API.Contracts.IRepositories
{
    public interface ITaskRepository
    {
        Task<TaskModel> GetByIdAsync(string id);
        Task AddAsync(TaskModel item);
        Task UpdateAsync(string Id, TaskModel item);
        Task DeleteAsync(string id);
        Task<List<TaskModel>> GetPaginatedAsync(int pageNumber,int pageSize,Expression<Func<TaskModel, bool>>? filter = null); 
        Task<long> CountAsync(Expression<Func<TaskModel, bool>>? filter = null);
        Task DeleteManyAsync(List<string> Ids);

    }
}