namespace Api.Tests
{
    using Api;
    using Api.Tests.ApplicationFactory;
    using FluentAssertions;
    using Newtonsoft.Json;
    using Api.Tests.Extensions;
    using System.Net;
    using Api.Tests.Helpers;
    using Application.Users.Commands;
    using Application.Users.Queries;
    using Domain.Exceptions;

    [TestCaseOrderer("Api.Tests.Helpers.PriorityOrderer", "Api.Tests")]
    public class UserTests : IClassFixture<PreferencesWebApplicationFactory<Program>>
    {
        private PreferencesWebApplicationFactory<Program> _factory;

        public UserTests(PreferencesWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        
        [Fact, TestPriority(1)]
        public async Task GetAllUsers_Tests()
        {
            var client = _factory.CreateClient();
            await _factory.InitSeed();
            var users = await GetAllUsersAsync(client);
            users.Count().Should().Be(2);
        }
        
        [Fact, TestPriority(2)]
        public async Task GetSolutionId_Tests()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.InitSeed();
        
            //Act
            var result = await GetUserByIdAsync(client, 1);
        
            // Assert
            result.Id.Should().Be(1);
            result.Name.Should().Be("User A");
        }
        
        [Fact, TestPriority(3)]
        public async Task GetUserIdThatDoesNotExist_Should_Throw_Exception_Tests()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.InitSeed();
        
            Func<Task> act = async () =>
            {
                await GetUserByIdAsync(client, 4);
            };
        
            // Act
            await act.Should().ThrowAsync<NotFoundException>();
        }
        
        [Fact, TestPriority(4)]
        public async Task CreateUserTest_Async()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.InitSeed();
            
            var command = new CreateUserCommand
            {
                Name = "New",
                Email = "test@test.com"
            };
        
            // Act
            var newUser = await CreateUserAsync(client, command);
        
            // Assert
            newUser.Should().NotBe(null);
            newUser.Name.Should().Be("New");
            newUser.Email.Should().Be("test@test.com");
        }
        
        [Fact, TestPriority(5)]
        public async Task UpdateUserTest_Async()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.InitSeed();
            var name = "Newsletter";
        
            var users = await GetAllUsersAsync(client);
            var firstUser = users.First();
        
            var command = new UpsertPreferencesToUserCommand
            {
                UserPreferences = new List<UserPreferenceDTO> { new UserPreferenceDTO { Name = name, Value = "False" } }
            };
        
            // Act
            var userResponse = await CreateOrUpdateUserPreferencesAsync(client, firstUser.Id, 1, command);
            
            // Assert
            userResponse.UserPreferences.Count().Should().Be(1);
            userResponse.UserPreferences.FirstOrDefault().Name.Should().Be(name);
        }
        
        [Fact, TestPriority(6)]
        public async Task UpdateSolutionWithInfoThatDoesNotExistOnGlobal_Should_Throw_Exception()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.InitSeed();
            var name = "Fake Mode";
        
            var users = await GetAllUsersAsync(client);
            var firstUser = users.First();
        
            var command = new UpsertPreferencesToUserCommand
            {
                UserPreferences = new List<UserPreferenceDTO> { new UserPreferenceDTO { Name = name, Value = "False" } }
            };
        
            Func<Task> act = async () =>
            {
                await CreateOrUpdateUserPreferencesAsync(client, firstUser.Id, 1, command);
            };
        
            // Act
            await act.Should().ThrowAsync<ApiException>();
        }

        [Fact, TestPriority(7)]
        public async Task DeleteUserTest_Async()
        {
            // Arrange
            var client = _factory.CreateClient();
            await _factory.InitSeed();

            var users = await GetAllUsersAsync(client);
            var firstUser = users.First();

            // Act
            var solutionResponse = await client.DeleteAsync($"api/users/{firstUser.Id}");

            // Assert
            solutionResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
        
        private async Task<IEnumerable<UserResponse>> GetAllUsersAsync(HttpClient client)
        {
            var response = await client.GetStringAsync($"api/users/");
            var users = JsonConvert.DeserializeObject<IEnumerable<UserResponse>>(response);
            return users;
        }
        
        private async Task<UserResponse> GetUserByIdAsync(HttpClient client, int userId)
        {
            var response = await client.GetStringAsync($"api/users/{userId}");
            var solution = JsonConvert.DeserializeObject<UserResponse>(response);
            return solution;
        }
        
        private async Task<UserResponse> CreateUserAsync(HttpClient client, CreateUserCommand command)
        {
            return await client.PostAsync<CreateUserCommand, UserResponse>($"api/users", command);
        }
        
        private async Task<UserResponse> CreateOrUpdateUserPreferencesAsync(HttpClient client, int userId, int solutionId, UpsertPreferencesToUserCommand command)
        {
            return await client.PostAsync<UpsertPreferencesToUserCommand, UserResponse>($"api/users/{userId}/solutions/{solutionId}/preferences", command);
        }
    }
}