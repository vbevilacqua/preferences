using System.Net.Http.Headers;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Microsoft.Extensions.Configuration;

namespace Api.Tests
{
    using Application.Solutions.Commands;
    using Api;
    using Api.Tests.ApplicationFactory;
    using Application.Solutions.Queries;
    using Application.SolutionsPreferences.Commands;
    using FluentAssertions;
    using Newtonsoft.Json;
    using Api.Tests.Extensions;
    using System.Net;
    using Api.Tests.Helpers;
    using Domain.Exceptions;

    [TestCaseOrderer("Api.Tests.Helpers.PriorityOrderer", "Api.Tests")]
    public class SolutionTests : IClassFixture<PreferencesWebApplicationFactory<Program>>
    {
        
        private readonly HttpClient httpClient;
        private readonly IConfigurationSection auth0Settings;
        private PreferencesWebApplicationFactory<Program> _factory;

        public SolutionTests(PreferencesWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            httpClient = factory.CreateClient();

            auth0Settings = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build()
                .GetSection("Auth0");
        }
        
        async Task<string> GetAccessToken()
        {
            var auth0Client = new AuthenticationApiClient(auth0Settings["Domain"]);
            var tokenRequest = new ClientCredentialsTokenRequest()
            {
                ClientId = auth0Settings["ClientId"],
                ClientSecret = auth0Settings["ClientSecret"],
                Audience = auth0Settings["Audience"]
            };
            var tokenResponse = await auth0Client.GetTokenAsync(tokenRequest);

            return tokenResponse.AccessToken;
        }

        [Fact, TestPriority(1)]
        public async Task GetAllSolutions_Tests()
        {
            var client = _factory.CreateClient();
            await _factory.InitSeed();
            var solutions = await GetAllSolutionsAsync(client);
            solutions.Count().Should().Be(3);
        }
        
        [Fact, TestPriority(2)]
        public async Task GetSolutionId_Tests()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.InitSeed();
            const int solutionId = 1;
            
            //Act
            var result = await GetSolutionByIdAsync(client, solutionId);
        
            // Assert
            result.Id.Should().Be(solutionId);
            result.Name.Should().Be("Game A");
        }
        
        [Fact, TestPriority(3)]
        public async Task GetSolutionIdThatDoesNotExist_Should_Throw_Exception_Tests()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.InitSeed();
        
            Func<Task> act = async () =>
            {
                await GetSolutionByIdAsync(client, 4);
            };
        
            // Act
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact, TestPriority(4)]
        public async Task CreateSolutionTest_Async()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.InitSeed();
            
            var command = new CreateSolutionCommand
            {
                Name = "New",
                Type = "Game"
            };

            // Act
            var newSolution = await CreateSolutionAsync(client, command);

            // Assert
            newSolution.Should().NotBe(null);
            newSolution.Name.Should().Be("New");
            newSolution.Type.Should().Be("Game");
        }
        
        [Fact, TestPriority(5)]
        public async Task UpdateSolutionTest_Async()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.InitSeed();
            var name = "Dark Mode";

            var solutions = await GetAllSolutionsAsync(client);
            var firstSolution = solutions.First();

            var command = new UpsertPreferencesToSolutionCommand
            {
                SolutionPreferences = new List<SolutionPreferenceDTO> { new SolutionPreferenceDTO { Name = name, Value = "False" } }
            };

            // Act
            var solutionResponse = await CreateOrUpdateSolutionPreferencesAsync(client, firstSolution.Id, command);

            // Assert
            solutionResponse.SolutionPreferences.Count().Should().Be(1);
            solutionResponse.SolutionPreferences.FirstOrDefault().Name.Should().Be(name);
        }

        [Fact, TestPriority(6)]
        public async Task UpdateSolutionWithInfoThatDoesNotExistOnGlobal_Should_Throw_Exception()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.InitSeed();
            var name = "Fake Mode";

            var solutions = await GetAllSolutionsAsync(client);
            var firstSolution = solutions.First();

            var command = new UpsertPreferencesToSolutionCommand
            {
                SolutionPreferences = new List<SolutionPreferenceDTO> { new SolutionPreferenceDTO { Name = name, Value = "False" } }
            };

            Func<Task> act = async () =>
            {
                await CreateOrUpdateSolutionPreferencesAsync(client, firstSolution.Id, command);
            };


            // Act
            await act.Should().ThrowAsync<ApiException>();
        }

        [Fact, TestPriority(7)]
        public async Task DeleteSolutionTest_Async()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.InitSeed();

            var solutions = await GetAllSolutionsAsync(client);
            var firstSolution = solutions.First();

            // Act
            var solutionResponse = await client.DeleteAsync($"api/solutions/{firstSolution.Id}");

            // Assert
            solutionResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        private async Task<IEnumerable<SolutionResponse>> GetAllSolutionsAsync(HttpClient client)
        {
            var response = await client.GetStringAsync($"api/solutions");
            var solutions = JsonConvert.DeserializeObject<IEnumerable<SolutionResponse>>(response);
            return solutions;
        }
        
        private async Task<SolutionResponse> GetSolutionByIdAsync(HttpClient client, int solutionId)
        {
            var response = await client.GetStringAsync($"api/solutions/{solutionId}");
            var solution = JsonConvert.DeserializeObject<SolutionResponse>(response);
            return solution;
        }
        
        private async Task<SolutionResponse> CreateSolutionAsync(HttpClient client, CreateSolutionCommand command)
        {
            return await client.PostAsync<CreateSolutionCommand, SolutionResponse>($"api/solutions", command);
        }
        
        private async Task<SolutionResponse> CreateOrUpdateSolutionPreferencesAsync(HttpClient client, int solutionId, UpsertPreferencesToSolutionCommand command)
        {
            return await client.PostAsync<UpsertPreferencesToSolutionCommand, SolutionResponse>($"api/solutions/{solutionId}/preferences", command);
        }
    }
}