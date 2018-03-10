using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace Test
{
    public class Test
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public Test()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            
            _client = _server.CreateClient();
        }
        
        [Fact]
        public async Task SmokeTest()
        {
            var response = await _client.GetAsync("/models");
            response.EnsureSuccessStatusCode();
        }
    }
}