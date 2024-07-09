using System.ComponentModel.DataAnnotations;
using ShortcutService.Constants;

namespace ShortcutService.Models;

public class ShortcutPath
{
    public readonly string Category;
    public readonly string Action;
    
    public ShortcutPath(string category, string action)
    {
        Category = category;
        Action = action;
    }
    
    public ShortcutPath(string path)
    {
        if (!ShortcutRegex.ActionRegex().IsMatch(path))
        {
            throw new ValidationException("Invalid path format.");
        }

        var parts = path.Split('.');
       
        Category = parts[0];
        Action = parts[1];
    }

    public override bool Equals(object? obj)
    {
        if (obj is ShortcutPath path)
        {
            return Category == path.Category && Action == path.Action;
        }

        return false;
    }
    
    public override int GetHashCode()
    {
        return Category.GetHashCode() ^ Action.GetHashCode();
    }

    public override string ToString()
    {
        return $"{Category}.{Action}";
    }
}