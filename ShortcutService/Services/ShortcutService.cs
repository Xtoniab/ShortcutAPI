using ShortcutService.Models;

namespace ShortcutService.Services;

public interface IShortcutService
{
    bool AddShortcut(Shortcut shortcut);
    bool DeleteShortcut(ShortcutBinding binding);
    IEnumerable<Shortcut> GetShortcutsByCategory(string category);
}

public class ShortcutService : IShortcutService
{
    private readonly List<Shortcut> _shortcuts = [];

    public bool AddShortcut(Shortcut shortcut)
    {
        foreach (var sc in _shortcuts)
        {
            if(sc.Binding.Equals(shortcut.Binding))
            {
                return false;
            }
        }
        
        _shortcuts.Add(shortcut);
        
        return true;
    }

    public bool DeleteShortcut(ShortcutBinding binding)
    {
        var shortcut = _shortcuts.FirstOrDefault(s => s.Binding.Equals(binding));
        
        if (shortcut != null)
        {
            _shortcuts.Remove(shortcut);
            return true;
        }
        
        return false;
    }

    public IEnumerable<Shortcut> GetShortcutsByCategory(string category)
    {
        return _shortcuts.Where(s => s.Path.Category == category);
    }
}