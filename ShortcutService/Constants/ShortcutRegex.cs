using System.Text.RegularExpressions;

namespace ShortcutService.Constants;

public static partial class ShortcutRegex
{
    public const string BindingPattern = @"^(Ctrl|Alt|Shift)(\s*\+\s*(Ctrl|Alt|Shift)){0,2}\s*\+\s*[A-Z]$";
    public const string ActionPattern = @"^[a-z]+\.[a-z]+$";
    
    [GeneratedRegex(BindingPattern)]
    public static partial Regex BindingRegex();
    
    [GeneratedRegex(ActionPattern)]
    public static partial Regex ActionRegex();
}