using Integration;
using Xunit;
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;
using ToDoList.API.utils.Responses;
using ToDoList.API.models;

namespace ToDoList.Tests.Integration;

public class IntegrationTest: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public IntegrationTest(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllTasks() 
    {
        HttpResponseMessage response = await _client.GetAsync("api/v1/todo");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task AddNewTask() 
    {
        TaskModel task = new TaskModel{ Name = "New Task", Description = "New Task", IsComplete = false };

        HttpResponseMessage response = await _client.PostAsJsonAsync("api/v1/todo", task);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<Response<TaskModel>>();

        content.Should().NotBeNull();
        content.Status.Should().Be("success");
        content.Message.Should().Be("Task created");
        content.data.Name.Should().NotBeNullOrEmpty();
        content.data.Name.Should().Be(task.Name);
        content.Code.Should().Be(201);
        content.data.Id.Should().BeOfType<string>();
        content.data.Id.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetTask()
    {
        TaskModel task = new TaskModel{ Name = "New Task", Description = "New Task", IsComplete = false };
        HttpResponseMessage response = await _client.PostAsJsonAsync("api/v1/todo", task);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        Response<TaskModel>? content = await response.Content.ReadFromJsonAsync<Response<TaskModel>>();

        content.Should().NotBeNull();
        content.data.Id.Should().NotBeNullOrEmpty();
        string id = content.data.Id;

        HttpResponseMessage getResponse = await _client.GetAsync($"api/v1/todo/{id}");

        Response<TaskModel>? getContent = await getResponse.Content.ReadFromJsonAsync<Response<TaskModel>>();

        getContent.Should().NotBeNull();

        getContent.data.Should().NotBeNull();
        getContent.data.Id.Should().Be(id);
        getContent.data.Name.Should().Be(task.Name);
        getContent.data.Description.Should().Be(task.Description);

        getContent.Status.Should().NotBeNull();
        getContent.Status.Should().Be("success");

        getContent.Message.Should().NotBeNull();
        getContent.Message.Should().Be("Task founded");

        getContent.Code.Should().NotBeNull();
        getContent.Code.Should().Be(200);
    }

    [Fact]
    public async Task DeleteTask() 
    {
        TaskModel task = new TaskModel{ Name = "New Task", Description = "New Task", IsComplete = false };

        HttpResponseMessage postResponse = await _client.PostAsJsonAsync("api/v1/todo", task);
        postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        Response<TaskModel>? postContent = await postResponse.Content.ReadFromJsonAsync<Response<TaskModel>>();
        
        postContent.Should().NotBeNull();
        postContent.data.Id.Should().NotBeNullOrEmpty();
        string id = postContent.data.Id;

        HttpResponseMessage deleteResponse = await _client.DeleteAsync($"api/v1/todo/{id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        Response<string>? deleteContent = await deleteResponse.Content.ReadFromJsonAsync<Response<string>>();

        deleteContent.Should().NotBeNull();

        deleteContent.Status.Should().NotBeNull();
        deleteContent.Status.Should().Be("success");

        deleteContent.Message.Should().NotBeNull();
        deleteContent.Message.Should().Be("Task deleted");

        deleteContent.Code.Should().NotBeNull();
        deleteContent.Code.Should().Be(200);
    }

    [Fact]
    public async Task ChangeStatusTask()
    {
        TaskModel task = new TaskModel{ Name = "New Task", Description = "New Task", IsComplete = false };
        HttpResponseMessage postResponse = await _client.PostAsJsonAsync("api/v1/todo", task);
        postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        Response<TaskModel>? postContent = await postResponse.Content.ReadFromJsonAsync<Response<TaskModel>>();

        postContent.Should().NotBeNull();
        postContent.data.Id.Should().NotBeNull();

        string Id = postContent.data.Id;

        HttpResponseMessage? changeResponse = await _client.GetAsync($"api/v1/todo/change/{Id}");
        changeResponse.Should().NotBeNull();
        changeResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        Response<TaskModel>? changeContent = await changeResponse.Content.ReadFromJsonAsync<Response<TaskModel>>();

        changeContent.Should().NotBeNull();
        changeContent.Should().BeOfType<TaskModel>();

        changeContent.Status.Should().NotBeNull();
        changeContent.Status.Should().Be("success");

        changeContent.Message.Should().NotBeNull();
        changeContent.Message.Should().Be("Task changed");

        changeContent.Code.Should().NotBeNull();
        changeContent.Code.Should().Be(200);

        changeContent.data.Id.Should().Be(Id);

        changeContent.data.Name.Should().Be(task.Name);

        changeContent.data.Description.Should().Be(task.Description);

        changeContent.data.IsComplete.Should().Be(!task.IsComplete);
    }

}
