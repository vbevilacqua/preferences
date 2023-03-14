namespace Api.Tests
{
    using System.Net;
    using Api;
    using Api.Tests.ApplicationFactory;
    using Api.Tests.Extensions;
    using Api.Tests.Helpers;
    using Application.Users.Commands;
    using Application.Users.Queries;
    using Domain.Exceptions;
    using FluentAssertions;

    [TestCaseOrderer("Api.Tests.Helpers.PriorityOrderer", "Api.Tests")]
    public class UserTests : IClassFixture<PreferencesWebApplicationFactory<Program>>, IAsyncLifetime
    {
        private PreferencesWebApplicationFactory<Program> _factory;
        private HttpClient client;

        public UserTests(PreferencesWebApplicationFactory<Program> factory)
        {
            this._factory = factory;
            this.client = _factory.CreateClient();
        }

        public async Task InitializeAsync()
        {
            await _factory.InitSeed();
        }

        public async Task DisposeAsync()
        {
            await _factory.DeleteDatabaseAsync();
        }

        [Fact, TestPriority(1)]
        public async Task GetAllUsers_Tests()
        {
            var users = await GetAllUsersAsync(client);
            users.Count().Should().Be(2);
        }

        [Fact, TestPriority(2)]
        public async Task GetSolutionId_Tests()
        {
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

            var users = await GetAllUsersAsync(client);
            var firstUser = users.First();

            // Act
            var solutionResponse = await client.DeleteAsyncWithHeader($"api/users/{firstUser.Id}");

            // Assert
            solutionResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        private Task<IEnumerable<UserResponse>> GetAllUsersAsync(HttpClient client) => client.GetAsync<IEnumerable<UserResponse>>($"api/users/");

        private Task<UserResponse> GetUserByIdAsync(HttpClient client, int userId) => client.GetAsync<UserResponse>($"api/users/{userId}");

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