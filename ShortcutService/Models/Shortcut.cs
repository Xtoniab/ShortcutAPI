using ShortcutService.DTOs;

namespace ShortcutService.Models;

public class Shortcut(ShortcutDto dto)
{
    public ShortcutPath Path { get; set; } = new(dto.Action);
    public string Description { get; set; } = dto.Description;
    public ShortcutBinding Binding { get; set; } = new(dto.Binding);
}