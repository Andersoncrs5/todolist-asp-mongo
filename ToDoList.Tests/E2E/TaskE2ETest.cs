using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using ToDoList.API.Controllers.DTOs;
using ToDoList.API.models;
using ToDoList.API.utils.pagination;
using ToDoList.API.utils.Responses;

namespace E2E
{
    public class TaskE2ETest: E2EBaseTest
    {
        public TaskE2ETest(WebApplicationFactory<Program> factory) : base(factory) {}

        private async Task CleanDatabaseAsync() 
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var mongoClient = scope.ServiceProvider.GetRequiredService<IMongoClient>();
                var settings = scope.ServiceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<ToDoList.API.utils.MongoDbSettings>>().Value;

                var database = mongoClient.GetDatabase(settings.DatabaseName);
                var collection = database.GetCollection<TaskModel>("Tasks"); 

                await collection.DeleteManyAsync(Builders<TaskModel>.Filter.Empty); 
            }
        }

        [Fact]
        public async Task CreateTask() 
        {
            var dto = new CreateTaskDTO
            {
                Name = "E2E test task",
                Description = "E2E test task",
                IsComplete = false
            };

            var response = await _client.PostAsJsonAsync("/api/v1/Todo",dto);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var taskCreated = await response.Content.ReadFromJsonAsync<Response<TaskModel>>();
            Assert.NotNull(taskCreated);
            Assert.False(string.IsNullOrEmpty(taskCreated.data.Id));
            Assert.Equal(dto.Name, taskCreated.data.Name);
            Assert.Equal(dto.Description, taskCreated.data.Description);
            Assert.Equal(dto.IsComplete, taskCreated.data.IsComplete);   
        }

        [Fact]
        public async Task GetTask() 
        {
            var dto = new CreateTaskDTO
            {
                Name = "E2E test task",
                Description = "E2E test task",
                IsComplete = false
            };

            HttpResponseMessage response = await _client.PostAsJsonAsync("/api/v1/Todo",dto);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Response<TaskModel>? contentResponse = await response.Content.ReadFromJsonAsync<Response<TaskModel>>();
            Assert.NotNull(contentResponse);
            Assert.NotNull(contentResponse.data.Id);

            string Id = contentResponse.data.Id;

            HttpResponseMessage getResponse = await _client.GetAsync($"/api/v1/Todo/{Id}");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Response<TaskModel>? getContent = await getResponse.Content.ReadFromJsonAsync<Response<TaskModel>>();

            Assert.NotNull(getContent);
            Assert.Equal(getContent.data.Id, contentResponse.data.Id);
            Assert.Equal(getContent.data.Name, contentResponse.data.Name);
            Assert.Equal(getContent.data.Description, contentResponse.data.Description);
            Assert.Equal(getContent.data.IsComplete, contentResponse.data.IsComplete);
        }

        [Fact]
        public async Task DeleteTask() 
        {
            CreateTaskDTO dto = new CreateTaskDTO
            {
                Name = "E2E test task",
                Description = "E2E test task",
                IsComplete = false
            };

            HttpResponseMessage response = await _client.PostAsJsonAsync("/api/v1/Todo",dto);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Response<TaskModel>? contentResponse = await response.Content.ReadFromJsonAsync<Response<TaskModel>>();
            Assert.NotNull(contentResponse);
            Assert.NotNull(contentResponse.data.Id);

            string Id = contentResponse.data.Id;

            HttpResponseMessage deleteResponse  = await _client.DeleteAsync($"/api/v1/Todo/{Id}");
            deleteResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);

            Response<string>? deleteContent = await deleteResponse.Content.ReadFromJsonAsync<Response<string>>();
            Assert.NotNull(deleteContent);

            Assert.Equal(deleteContent.Message, "Task deleted");
            Assert.Equal(deleteContent.Code, 200);
        }

        [Fact]
        public async Task UpdateTask() {
            CreateTaskDTO dto = new CreateTaskDTO
            {
                Name = "E2E test task",
                Description = "E2E test task",
                IsComplete = false
            };

            HttpResponseMessage response = await _client.PostAsJsonAsync("/api/v1/Todo",dto);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Response<TaskModel>? contentResponse = await response.Content.ReadFromJsonAsync<Response<TaskModel>>();
            Assert.NotNull(contentResponse);
            Assert.NotNull(contentResponse.data.Id);

            string Id = contentResponse.data.Id;

            UpdateTaskDTO uDto = new UpdateTaskDTO
            {
                Name = "E2E test task update",
                Description = "E2E test task update",
                IsComplete = true
            };

            HttpResponseMessage putResponse = await _client.PutAsJsonAsync($"/api/v1/Todo/{Id}",uDto);
            putResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);

            Response<string>? putContent = await putResponse.Content.ReadFromJsonAsync<Response<string>>();
            Assert.NotNull(putContent);
            Assert.Equal(putContent.Message, "Task updated");
            Assert.Equal(putContent.Code, 200);

            HttpResponseMessage getResponse = await _client.GetAsync($"/api/v1/Todo/{Id}");
            getResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            Response<TaskModel>? getContent = await getResponse.Content.ReadFromJsonAsync<Response<TaskModel>>();
            Assert.NotNull(getContent);
            Assert.Equal(getContent.data.Id, Id);
            Assert.Equal(getContent.data.Name, uDto.Name);
            Assert.Equal(getContent.data.Description, uDto.Description);
            Assert.Equal(getContent.data.IsComplete, uDto.IsComplete);
        }

        [Fact]
        public async Task ChangeStatusTask() 
        {
            CreateTaskDTO dto = new CreateTaskDTO
            {
                Name = "E2E test task",
                Description = "E2E test task",
                IsComplete = false
            };

            HttpResponseMessage response = await _client.PostAsJsonAsync("/api/v1/Todo",dto);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Response<TaskModel>? contentResponse = await response.Content.ReadFromJsonAsync<Response<TaskModel>>();
            Assert.NotNull(contentResponse);
            Assert.NotNull(contentResponse.data.Id);

            string Id = contentResponse.data.Id;

            HttpResponseMessage changeResponse = await _client.GetAsync($"/api/v1/Todo/change/{Id}");
            changeResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, changeResponse.StatusCode);

            HttpResponseMessage getResponse = await _client.GetAsync($"/api/v1/Todo/{Id}");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Response<TaskModel>? getContent = await getResponse.Content.ReadFromJsonAsync<Response<TaskModel>>();

            Assert.NotNull(getContent);
            Assert.Equal(getContent.data.Id, contentResponse.data.Id);
            Assert.Equal(getContent.data.Name, contentResponse.data.Name);
            Assert.Equal(getContent.data.Description, contentResponse.data.Description);
            Assert.NotEqual(getContent.data.IsComplete, contentResponse.data.IsComplete);
        }

    }
    
}