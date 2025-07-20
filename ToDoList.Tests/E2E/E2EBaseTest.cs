using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using ToDoList.API;

namespace E2E
{
    public class E2EBaseTest: IClassFixture<WebApplicationFactory<Program>>
    {
        protected readonly HttpClient _client;
        protected readonly WebApplicationFactory<Program> _factory;

        public E2EBaseTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseSolutionRelativeContentRoot("ToDoList.API")
                       .UseEnvironment("Development");

                builder.ConfigureAppConfiguration((context, conf) =>
                {
                    conf.Sources.Clear();

                    conf.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddEnvironmentVariables();
                });
            });

            _client = _factory.CreateClient();
        }
    }
}