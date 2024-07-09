using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using ShortcutService.DTOs;
using ShortcutService.Models;
using ShortcutService.Services;

namespace ShortcutService.IntegrationTests;

[Collection("Integration Tests")]
public class ShortcutsControllerTests : IClassFixture<WebApplicationFactory<Startup>>
{
    public class AddResponse
    {
        public bool Success { get; set; }
    }
    
    public class GetResponseEntry
    {
        public string ActionName { get; set; }
        public string Binding { get; set; }
    }

    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Startup> _factory;
    private readonly IShortcutService _shortcutService;

    public ShortcutsControllerTests(WebApplicationFactory<Startup> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _shortcutService = factory.Services.GetService(typeof(IShortcutService)) as IShortcutService;
    }

    [Theory]
    [InlineData("Ctrl + Shift + T", "Open new tab", "browser.newtab")]
    [InlineData("Alt + Shift + N", "Open new window", "browser.newwindow")]
    [InlineData("Ctrl + Alt + Z", "Task Manager", "system.taskmanager")]
    public async Task AddShortcut_ReturnsOkResult_WhenShortcutIsValid(string binding, string description, string action)
    {
        // Arrange
        var shortcutDto = new ShortcutDto(binding, description, action);

        // Act
        var response = await _client.PostAsJsonAsync("/Shortcuts/add", shortcutDto);
    
        // Assert
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadFromJsonAsync<AddResponse>();
        responseContent.Success.Should().BeTrue();
        
        // Cleanup
        _shortcutService.DeleteShortcut(new ShortcutBinding(binding));
    }

    [Theory]
    [InlineData("Ctrl + Shift", "Incomplete shortcut", "browser.incomplete")]
    [InlineData("Ctrl + Shift + 1", "Invalid key", "browser.invalidkey")]
    [InlineData("Ctrl + Shift + Z + W", "Too many keys", "browser.toomanykeys")]
    public async Task AddShortcut_ReturnsBadRequest_WhenShortcutIsInvalid(string binding, string description, string action)
    {
        // Arrange
        var invalidShortcutDto = new ShortcutDto(binding, description, action);

        // Act
        var response = await _client.PostAsJsonAsync("/Shortcuts/add", invalidShortcutDto);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var responseContent = await response.Content.ReadFromJsonAsync<AddResponse>();
        responseContent.Success.Should().BeFalse();
    }

    [Theory]
    [InlineData("browser", "Ctrl + Shift + T", "Open new tab", "newtab")]
    [InlineData("browser", "Ctrl + Shift + N", "Open new window", "newwindow")]
    public async Task GetShortcutsByCategory_ReturnsShortcuts_WhenCategoryExists(string category, string binding, string description, string actionName)
    {
        // Arrange
        var shortcutDto = new ShortcutDto(binding, description, $"{category}.{actionName}");
        await _client.PostAsJsonAsync("/Shortcuts/add", shortcutDto);

        // Act
        var response = await _client.GetAsync($"/Shortcuts/category/{category}");

        // Assert
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadFromJsonAsync<GetResponseEntry[]>();
        responseContent.Should().ContainEquivalentOf(new GetResponseEntry
        {
            ActionName = actionName,
            Binding = binding
        });
        
        // Cleanup
        _shortcutService.DeleteShortcut(new ShortcutBinding(binding));
    }

    [Fact]
    public async Task GetShortcutsByCategory_ReturnsEmpty_WhenCategoryDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync("/Shortcuts/category/nonexistent");

        // Assert
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadFromJsonAsync<GetResponseEntry[]>();
        responseContent.Should().BeEmpty();
    }

    [Fact]
    public async Task AddShortcut_ReturnsFalse_WhenShortcutAlreadyExists()
    {
        // Arrange
        var shortcutDto = new ShortcutDto("Ctrl + Shift + T", "Open new tab", "browser.newtab");
        await _client.PostAsJsonAsync("/Shortcuts/add", shortcutDto);

        // Act
        var response = await _client.PostAsJsonAsync("/Shortcuts/add", shortcutDto);

        // Assert
        var responseContent = await response.Content.ReadFromJsonAsync<AddResponse>();
        responseContent.Success.Should().BeFalse();
        
        // Cleanup
        _shortcutService.DeleteShortcut(new ShortcutBinding("Ctrl + Shift + T"));
    }

    [Theory]
    [InlineData("Ctrl + Shift + K", "Push current branch to remote repository", "git.push")]
    [InlineData("Ctrl + T", "Fetch latest changes", "git.fetch")]
    public async Task AddShortcut_And_GetShortcutsByCategory(string binding, string description, string action)
    {
        // Arrange
        var shortcutDto = new ShortcutDto(binding, description, action);
        await _client.PostAsJsonAsync("/Shortcuts/add", shortcutDto);

        // Act
        var getResponse = await _client.GetAsync($"/Shortcuts/category/{action.Split('.')[0]}");

        // Assert
        getResponse.EnsureSuccessStatusCode();
        var responseContent = await getResponse.Content.ReadFromJsonAsync<GetResponseEntry[]>();
        responseContent.Should().ContainEquivalentOf(new GetResponseEntry
        {
            ActionName = action.Split('.')[1],
            Binding = binding
        });
        
        // Cleanup
        _shortcutService.DeleteShortcut(new ShortcutBinding(binding));
    }
}
