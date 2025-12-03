using manager; 
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;

namespace test
{
    public class ManagerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public ManagerIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async void Get_WhenEmpty_ReturnEmptyList()
        {
            var act = await _client.GetAsync("/api/Blacklist");
            Assert.True(act.IsSuccessStatusCode);

            var jsonString = await act.Content.ReadAsStringAsync();
            var list = JsonSerializer.Deserialize<List<string>>(jsonString);

            Assert.NotNull(list);
            Assert.Empty(list);
        }

        [Fact]
        public async void Post_WhenWordAdded_ReturnsInList()
        {
            var postResponse = await _client.PostAsync("/api/Blacklist?word=kurde", null);

            Assert.True(postResponse.IsSuccessStatusCode);

            var getResponse = await _client.GetAsync("/api/Blacklist");
            getResponse.EnsureSuccessStatusCode();

            var jsonString = await getResponse.Content.ReadAsStringAsync();
            var list = JsonSerializer.Deserialize<List<string>>(jsonString);

            Assert.Contains("kurde", list);
        }
    }
}