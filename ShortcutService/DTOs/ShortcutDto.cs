using System.ComponentModel.DataAnnotations;
using ShortcutService.Constants;

namespace ShortcutService.DTOs;

public class ShortcutDto(string binding, string description, string action)
{
    [Required]
    [RegularExpression(ShortcutRegex.BindingPattern, ErrorMessage = "Binding is invalid.")]
    public string Binding { get; } = binding;

    [Required]
    [StringLength(100)]
    public string Description { get; } = description;

    [Required]
    [RegularExpression(ShortcutRegex.ActionPattern, ErrorMessage = "Action is invalid.")]
    public string Action { get; } = action;
}