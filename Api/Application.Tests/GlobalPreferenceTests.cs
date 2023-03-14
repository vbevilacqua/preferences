using Application.GlobalPreferences.Commands;
using Application.GlobalPreferences.Queries;

namespace Api.Tests
{
    using Api;
    using Api.Tests.ApplicationFactory;
    using FluentAssertions;
    using Newtonsoft.Json;
    using Api.Tests.Extensions;
    using System.Net;
    using Api.Tests.Helpers;
    using Domain.Exceptions;

    [TestCaseOrderer("Api.Tests.Helpers.PriorityOrderer", "Api.Tests")]
    public class GlobalPreferenceTests : IClassFixture<PreferencesWebApplicationFactory<Program>>
    {
        private PreferencesWebApplicationFactory<Program> _factory;

        public GlobalPreferenceTests(PreferencesWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        
        [Fact, TestPriority(1)]
        public async Task GetAllGlobalPreferences_Tests()
        {
            var client = _factory.CreateClient();
            await _factory.InitSeed();
            var preferences = await GetAllGlobalPreferencesAsync(client);
            preferences.Count().Should().Be(3);
        }
        
        [Fact, TestPriority(2)]
        public async Task CreateGlobalPreferenceTest_Async()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.InitSeed();
            
            var command = new CreateGlobalPreferenceCommand
            {
                Name = "test",
                Value = "test",
                IsActive = false
            };
        
            // Act
            var newGlobalPreference = await CreateGlobalPreferenceAsync(client, command);
        
            // Assert
            newGlobalPreference.Should().NotBe(null);
            newGlobalPreference.Name.Should().Be("test");
            newGlobalPreference.Value.Should().Be("test");
            newGlobalPreference.IsActive.Should().Be(false);
        }
        
        [Fact, TestPriority(3)]
        public async Task UpdateGlobalPreferenceTest_Async()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.InitSeed();
            string name = "Newsletter";
        
            var preferences = await GetAllGlobalPreferencesAsync(client);
            var firstPreference = preferences.First();
        
            var command = new UpdateGlobalPreferenceCommand
            {
               Name = name, 
               Value = "False", 
               IsActive = false
            };
        
            // Act
            await UpdateGlobalPreferencesAsync(client, name, command);
        }
        
        [Fact, TestPriority(4)]
        public async Task DeleteGlobalPreferenceTest_Async()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.InitSeed();

            var preferences = await GetAllGlobalPreferencesAsync(client);
            var preference = preferences.First();

            // Act
            var solutionResponse = await client.DeleteAsync($"api/preferences/{preference.Name}");

            // Assert
            solutionResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
        
        private async Task<IEnumerable<GlobalPreferenceResponse>> GetAllGlobalPreferencesAsync(HttpClient client)
        {
            var response = await client.GetStringAsync($"api/preferences/");
            var preferences = JsonConvert.DeserializeObject<IEnumerable<GlobalPreferenceResponse>>(response);
            return preferences;
        }
        
        private async Task<GlobalPreferenceResponse> CreateGlobalPreferenceAsync(HttpClient client, CreateGlobalPreferenceCommand command)
        {
            return await client.PostAsync<CreateGlobalPreferenceCommand, GlobalPreferenceResponse>($"api/preferences", command);
        }
        
        private async Task<GlobalPreferenceResponse> UpdateGlobalPreferencesAsync(HttpClient client, string name, UpdateGlobalPreferenceCommand command)
        {
            return await client.PutAsync<UpdateGlobalPreferenceCommand, GlobalPreferenceResponse>($"api/preferences/{name}", command);
        }
    }
}