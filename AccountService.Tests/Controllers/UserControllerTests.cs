using System.Net;
using System.Net.Http.Json;
using AccountService.Tests.Controllers.Utils;
using AccountService.Utils.Result;

namespace AccountService.Tests.Controllers;

public class UserControllerTests : IntegrationTests
{

    public UserControllerTests(ApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task FindAllUsers_Success()
    {
        //Arrange
        var client = Factory.CreateClient();
        const int expected = 5;

        //Act
        var response = await client.GetAsync("/users");

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<MbResponse<List<Guid>>>();
        Assert.NotNull(result);
        Assert.NotNull(result.Result);
        Assert.Equal(expected, result.Result.Count);
    }

    [Fact]
    public async Task VerifyUser_ResultTrue()
    {
        //Arrange
        var client = Factory.CreateClient();
        var id = Guid.Parse("278857fc-746b-42ec-96fd-1e1bea494f69");
        const bool expected = true;

        //Act
        var response = await client.PostAsync($"/users/{id}/verify", null);

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<MbResponse<bool>>();
        Assert.NotNull(result);
        Assert.NotNull(result.Result);
        Assert.Equal(expected, result.Result);
    }

    [Fact]
    public async Task VerifyUser_ResultFalse()
    {
        //Arrange
        var client = Factory.CreateClient();
        var id = Guid.Parse("00000000-0000-0000-0000-000000000000");
        const bool expected = false;

        //Act
        var response = await client.PostAsync($"/users/{id}/verify", null);

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<MbResponse<bool>>();
        Assert.NotNull(result);
        Assert.NotNull(result.Result);
        Assert.Equal(expected, result.Result);
    }
}