using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

public class ApiValidationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    public ApiValidationTests(WebApplicationFactory<Program> factory) => _client = factory.CreateClient();

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Post_WithInvalidTitle_Returns400_ProblemDetails(string title)
    {
        var resp = await _client.PostAsJsonAsync("/api/todo", new { title });
        resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problem = await resp.Content.ReadFromJsonAsync<ProblemDetails>();
        problem.Should().NotBeNull();
        problem!.Status.Should().Be(400);
    }

    [Fact]
    public async Task Post_MissingTitleField_Returns400()
    {
        var resp = await _client.PostAsJsonAsync("/api/todo", new { });
        resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problem = await resp.Content.ReadFromJsonAsync<ProblemDetails>();
        problem.Should().NotBeNull();
        problem!.Status.Should().Be(400);
    }

    [Fact]
    public async Task Post_TitleTooLong_Returns400()
    {
        var longTitle = new string('a', 300);
        var resp = await _client.PostAsJsonAsync("/api/todo", new { title = longTitle });
        resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problem = await resp.Content.ReadFromJsonAsync<ProblemDetails>();
        problem.Should().NotBeNull();
        problem!.Status.Should().Be(400);
    }

    [Fact]
    public async Task Post_TitleAsNumber_Returns400()
    {
        var content = new StringContent("{ \"title\": 123 }", System.Text.Encoding.UTF8, "application/json");
        var resp = await _client.PostAsync("/api/todo", content);
        resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problem = await resp.Content.ReadFromJsonAsync<ProblemDetails>();
        problem.Should().NotBeNull();
        problem!.Status.Should().Be(400);
    }
}
