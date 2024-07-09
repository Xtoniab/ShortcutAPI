using Microsoft.AspNetCore.Mvc;
using ShortcutService.DTOs;
using ShortcutService.Models;
using ShortcutService.Services;

namespace ShortcutService.Controllers;

[ApiController]
[Route("[controller]")]
public class ShortcutsController(IShortcutService shortcutService) : ControllerBase
{
    [HttpPost("add")]
    public IActionResult AddShortcut([FromBody] ShortcutDto shortcutDto)
    {
        var shortcut = new Shortcut(shortcutDto);

        var result = shortcutService.AddShortcut(shortcut);
        if (result)
        {
            return Ok(new { success = true });
        }

        return BadRequest(new { success = false });
    }

    [HttpGet("category/{categoryName}")]
    public IActionResult GetShortcutsByCategory(string categoryName)
    {
        var shortcuts = shortcutService.GetShortcutsByCategory(categoryName);

        var result = shortcuts.Select(s => new 
        {
            ActionName = s.Path.Action,
            Binding = s.Binding.ToString()
        }).ToList();

        return Ok(result);
    }
}