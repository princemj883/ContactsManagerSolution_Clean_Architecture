using FluentAssertions;

namespace ContractsManager.IntegrationTests;

public class PersonsIntegrationTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task ShouldReturnAllPersons()
    {
        //Arrange

        //Act
        HttpResponseMessage response = await _client.GetAsync("/api/person");

        //Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        
    }
}