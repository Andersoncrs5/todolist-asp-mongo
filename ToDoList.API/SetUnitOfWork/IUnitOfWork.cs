using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.API.Contracts.IRepositories;
using ToDoList.API.Contracts.Repositories;

namespace ToDoList.API.SetUnitOfWork
{
    public interface IUnitOfWork: IDisposable
    {
        ITaskRepository TaskRepository { get; }
        Task<bool> CompleteAsync();

    }
}