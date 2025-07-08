using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ToDoList.API.Contracts.IRepositories;
using ToDoList.API.Contracts.Repositories;
using ToDoList.API.utils;

namespace ToDoList.API.SetUnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        
        private readonly IMongoClient _mongoClient;
        private readonly IOptions<MongoDbSettings> _mongoDbSettings;

        private ITaskRepository _taskRepository;

        public UnitOfWork(IMongoClient mongoClient, IOptions<MongoDbSettings> mongoDbSettings)
        {
            _mongoClient = mongoClient;
            _mongoDbSettings = mongoDbSettings;
        }

        public ITaskRepository TaskRepository
        {
            get
            {
                return _taskRepository ??= new TaskRepository(_mongoClient, _mongoDbSettings);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Task<bool> CompleteAsync()
        {
            return Task.FromResult(true);
        }
    }

}