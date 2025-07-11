using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using ToDoList.API.Contracts.IRepositories;
using ToDoList.API.models;
using ToDoList.API.utils;
using ToDoList.API.utils.ResponseException;

namespace ToDoList.API.Contracts.Repositories
{
    public class TaskRepository: ITaskRepository
    {
        private readonly IMongoCollection<TaskModel> _collection;

        public TaskRepository(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _collection = database.GetCollection<TaskModel>("Tasks");
        }

        public async Task<TaskModel> GetByIdAsync(string Id) 
        {
            if (string.IsNullOrEmpty(Id)) 
                throw new ResponseException("Id is required");

            TaskModel? task = await this._collection.Find(t => t.Id == Id).FirstOrDefaultAsync();

            if (task == null)
                throw new ResponseException("Task not found", 404);

            return task;
        }

        public async Task DeleteAsync(string Id) 
        {
            if (string.IsNullOrEmpty(Id)) 
                throw new ResponseException("Id is required");

            await this._collection.DeleteOneAsync(item => item.Id == Id);
        }

        public async Task DeleteManyAsync(List<string> Ids) 
        {
            if (Ids == null || !Ids.Any() || Ids.Count <= 0)
                return;

            List<ObjectId> objectIds = Ids.Select(Id => ObjectId.Parse(Id)).ToList();
            FilterDefinition<TaskModel> filter = Builders<TaskModel>.Filter.In("_id", objectIds);

            await _collection.DeleteManyAsync(filter);
        }

        public async Task<TaskModel> AddAsync(TaskModel task)
        {
            task.IsComplete = false;
            await _collection.InsertOneAsync(task);
            return task;
        }

        public async Task UpdateAsync(string Id, TaskModel task)
        {
            task.Id = Id;
            await this._collection.ReplaceOneAsync(t => t.Id == Id, task);
        }

        public async Task<List<TaskModel>> GetPaginatedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<TaskModel, bool>>? filter = null)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            int skipAmount = (pageNumber - 1) * pageSize;

            var query = filter == null
                ? _collection.Find(_ => true)
                : _collection.Find(filter);

            return await query.Skip(skipAmount).Limit(pageSize).ToListAsync();
        }

        public async Task<long> CountAsync(Expression<Func<TaskModel, bool>>? filter = null)
        {
            return filter == null
                ? await _collection.CountDocumentsAsync(_ => true)
                : await _collection.CountDocumentsAsync(filter);
        }

        public async Task ChangeComepleteTask(string Id, TaskModel task)
        {
            task.IsComplete = !task.IsComplete;
            await _collection.ReplaceOneAsync(item => item.Id.Equals(Id), task);
        }

    }
}