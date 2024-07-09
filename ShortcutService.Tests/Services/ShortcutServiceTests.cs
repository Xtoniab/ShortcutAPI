using System.ComponentModel.DataAnnotations;
using ShortcutService.DTOs;
using ShortcutService.Models;
using FluentAssertions;

namespace ShortcutService.Tests.Services
{
    public class ShortcutServiceTests
    {
        private readonly ShortcutService.Services.ShortcutService _service = new();

        [Theory]
        [InlineData("Ctrl+Alt+S", "Save File", "file.save", true)]
        [InlineData("Ctrl+Shift+D", "Save As File", "file.save", true)]
        [InlineData("Ctrl+Alt+S", "Open File", "file.open", true)]
        public void AddShortcut_ReturnsExpectedResult(string binding, string description, string path, bool expected)
        {
            // Clean up
            _service.ClearShortcuts();
            
            // Arrange
            var shortcutDto = new ShortcutDto(binding, description, path);
            var shortcut = new Shortcut(shortcutDto);

            // Act
            var result = _service.AddShortcut(shortcut);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void GetShortcutsByCategory_ReturnsShortcuts()
        {
            // Clean up
            _service.ClearShortcuts();
            
            // Arrange
            var shortcutDto1 = new ShortcutDto("Ctrl+Alt+S", "Save File", "file.save");
            var shortcut1 = new Shortcut(shortcutDto1);
            _service.AddShortcut(shortcut1);

            var shortcutDto2 = new ShortcutDto("Ctrl+Alt+O", "Open File", "file.open");
            var shortcut2 = new Shortcut(shortcutDto2);
            _service.AddShortcut(shortcut2);

            // Act
            var result = _service.GetShortcutsByCategory("file");

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public void GetShortcutsByCategory_ReturnsEmptyList_WhenCategoryDoesNotExist()
        {
            // Clean up
            _service.ClearShortcuts();
            
            // Act
            var result = _service.GetShortcutsByCategory("nonexistent");

            // Assert
            result.Should().BeEmpty();
        }

        [Theory]
        [InlineData("Alt+     Shift+T", "Alt + Shift + T")]
        [InlineData("Ctrl+T", "Ctrl + T")]
        public void ShortcutBinding_ParsesCorrectly(string binding, string expected)
        {
            // Clean up
            _service.ClearShortcuts();
            
            // Arrange
            var shortcutDto = new ShortcutDto(binding, "Test Binding", "category.action");
            var shortcut = new Shortcut(shortcutDto);

            // Act
            var bindingString = shortcut.Binding.ToString();

            // Assert
            bindingString.Should().Be(expected);
        }

        [Theory]
        [InlineData("T")]
        [InlineData("Ctrl++A")]
        public void ShortcutBinding_ThrowsException_WithInvalidFormat(string binding)
        {
            // Clean up
            _service.ClearShortcuts();
            
            // Arrange
            var shortcutDto = new ShortcutDto(binding, "Test Binding", "category.action");

            // Act & Assert
            Action act = () => new Shortcut(shortcutDto);
            act.Should().Throw<ValidationException>();
        }
    }
}
