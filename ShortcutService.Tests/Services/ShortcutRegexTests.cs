using System.Text.RegularExpressions;
using static ShortcutService.Constants.ShortcutRegex;

namespace ShortcutService.Tests.Services;

public class ShortcutRegexTests
{
    [Theory]
    [InlineData("Ctrl+Alt+S", true)]
    [InlineData("Alt+Shift+T", true)]
    [InlineData("Ctrl+Shift+X", true)]
    [InlineData("Ctrl+Alt+Shift+P", true)]
    [InlineData("Ctrl++A", false)]
    [InlineData("T", false)]
    [InlineData("Ctrl+Shift+1", false)]
    [InlineData("Ctrl+Shift+FS", false)]
    public void BindingPattern_Test(string binding, bool expectedIsValid)
    {
        var isValid = BindingRegex().IsMatch(binding);
        Assert.Equal(expectedIsValid, isValid);
    }

    [Theory]
    [InlineData("file.save", true)]
    [InlineData("edit.cut", true)]
    [InlineData("file.saveAs", false)]
    [InlineData("file-", false)]
    public void ActionPattern_Test(string action, bool expectedIsValid)
    {
        var isValid = ActionRegex().IsMatch(action);
        Assert.Equal(expectedIsValid, isValid);
    }
}