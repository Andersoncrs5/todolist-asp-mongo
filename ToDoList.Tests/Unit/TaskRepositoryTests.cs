using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using ToDoList.API.Contracts.Repositories;
using ToDoList.API.models;
using ToDoList.API.utils;
using ToDoList.API.utils.ResponseException;

namespace Unit
{
    public class TaskRepositoryTests
    {
        private readonly Mock<IMongoCollection<TaskModel>> _collectionMock;
        private readonly TaskRepository _repository;

        public TaskRepositoryTests()
        {
            _collectionMock = new Mock<IMongoCollection<TaskModel>>();

            var settingsMock = new Mock<IOptions<MongoDbSettings>>();
            settingsMock.Setup(s => s.Value).Returns(new MongoDbSettings
            {
                DatabaseName = "FakeDB"
            });

            var mongoDatabaseMock = new Mock<IMongoDatabase>();
            mongoDatabaseMock
                .Setup(db => db.GetCollection<TaskModel>("Tasks", null))
                .Returns(_collectionMock.Object);

            var mongoClientMock = new Mock<IMongoClient>();
            mongoClientMock
                .Setup(client => client.GetDatabase("FakeDB", null))
                .Returns(mongoDatabaseMock.Object);

            _repository = new TaskRepository(mongoClientMock.Object, settingsMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync()
        {
            Func<Task> act = async () => await _repository.GetByIdAsync("");

            await act.Should().ThrowAsync<ResponseException>().WithMessage("Id is required");
        }

        [Fact]
        public async Task ShouldThrowResponseExceptionInDelete() 
        {
            Func<Task> act = async () => await _repository.DeleteAsync("");

            await act.Should().ThrowAsync<ResponseException>().WithMessage("Id is required");
        }

        [Fact]
        public async Task ShouldNoThrowResponseExceptionInDeleteMany()
        {
            Func<Task> act = async () => await _repository.DeleteManyAsync(new List<string>());

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task AddAsync()
        {
            var task = new TaskModel { Id = ObjectId.GenerateNewId().ToString(), Name = "Test", Description = "Test desc" };
            _collectionMock.Setup(c => c.InsertOneAsync(task, null, default)).Returns(Task.CompletedTask);

            TaskModel result = await _repository.AddAsync(task);
            result.IsComplete.Should().BeFalse();
            result.Name.Should().Be(task.Name);
            result.Description.Should().Be(task.Description);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            var task = new TaskModel { Name = "Updated Task" };
            var id = ObjectId.GenerateNewId().ToString();

            _collectionMock.Setup(c => c.ReplaceOneAsync(
                It.IsAny<FilterDefinition<TaskModel>>(),
                task,
                It.IsAny<ReplaceOptions>(),
                default))
            .ReturnsAsync(ReplaceOneResult.Unacknowledged.Instance);

            await _repository.UpdateAsync(id, task);

            task.Id.Should().Be(id);
        }

        [Fact]
        public async Task ChangeCompleteTask()
        {
            var task = new TaskModel { Id = "123", IsComplete = false };

            _collectionMock.Setup(c => c.ReplaceOneAsync(
                It.IsAny<FilterDefinition<TaskModel>>(),
                It.IsAny<TaskModel>(),
                It.IsAny<ReplaceOptions>(),
                default))
            .ReturnsAsync(ReplaceOneResult.Unacknowledged.Instance);

            await _repository.ChangeComepleteTask(task.Id, task);

            task.IsComplete.Should().BeTrue();

        }

        [Fact]
        public async Task CountAsync()
        {
            _collectionMock.Setup(c => c.CountDocumentsAsync(
                It.IsAny<FilterDefinition<TaskModel>>(), 
                null, 
                default))
            .ReturnsAsync(42);

            long result = await _repository.CountAsync();

            result.Should().Be(42);
        }

    }
}