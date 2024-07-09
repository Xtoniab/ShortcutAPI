using System.ComponentModel.DataAnnotations;
using ShortcutService.Constants;


namespace ShortcutService.Models;

public class ShortcutBinding
{
    public readonly List<SystemKey> ModifierKeys;
    public readonly char Key;
    
    public ShortcutBinding(List<SystemKey> modifierKeys, char key)
    {
        if (modifierKeys == null || !modifierKeys.Any() || key == '\0')
        {
            throw new ValidationException("Invalid binding: must contain at least one system key and one character.");
        }
        
        ModifierKeys = modifierKeys;
        Key = key;
    }
    
    public ShortcutBinding(string binding)
    {
        if (!ShortcutRegex.BindingRegex().IsMatch(binding))
        {
            throw new ValidationException("Invalid binding format: must be in the form of 'Ctrl + Shift + A'.");
        }
        
        ModifierKeys = new List<SystemKey>();

        var keys = binding.Split('+').Select(k => k.Trim()).ToList();

        foreach (var key in keys.Take(keys.Count - 1))
        {
            if (Enum.TryParse<SystemKey>(key, true, out var modifierKey))
            {
                ModifierKeys.Add(modifierKey);
            }
        }

        Key = keys.Last()[0];
    }

    public override bool Equals(object? obj)
    {
        if (obj is ShortcutBinding binding)
        {
            return ModifierKeys.SequenceEqual(binding.ModifierKeys) && Key == binding.Key;
        }

        return false;
    }
    
    public override int GetHashCode()
    {
        return ModifierKeys.Aggregate(0, (hash, key) => hash ^ key.GetHashCode()) ^ Key.GetHashCode();
    }

    public override string ToString()
    {
        var result = string.Join(" + ", ModifierKeys.Select(k => k.ToString()));
        if (Key != '\0')
        {
            result += " + " + Key;
        }

        return result;
    }


}