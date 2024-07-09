using System.Collections.Concurrent;
using ShortcutService.Models;

namespace ShortcutService.Services;

public interface IShortcutService
{
    bool AddShortcut(Shortcut shortcut);
    bool DeleteShortcut(ShortcutBinding binding);
    IEnumerable<Shortcut> GetShortcutsByCategory(string category);
    void ClearShortcuts();
}

public class ShortcutService : IShortcutService
{
    private readonly ConcurrentDictionary<ShortcutBinding, Shortcut> _shortcuts = [];

    public bool AddShortcut(Shortcut shortcut)
    {
        if (!_shortcuts.TryAdd(shortcut.Binding, shortcut))
        {
            return false;
        }

        return true;
    }

    public bool DeleteShortcut(ShortcutBinding binding)
    {
        return _shortcuts.Remove(binding, out _);
    }

    public IEnumerable<Shortcut> GetShortcutsByCategory(string category)
    {
        return _shortcuts.Values.Where(s => s.Path.Category == category);
    }

    public void ClearShortcuts()
    {
        _shortcuts.Clear();
    }
}