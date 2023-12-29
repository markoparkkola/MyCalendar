using CalendarTerminal;
using Cocona;
using Core.Services;

public class TerminalCommands
{
  private readonly TerminalService _terminalService;

  public TerminalCommands(ICalendarService calendarService)
  {
    _terminalService = new TerminalService(calendarService);
  }
  
  [Command("list")]
  public async Task ListEntries()
  {
    await _terminalService.ShowEntries(null, null);
  }

  [Command("find")]
  public async Task FindEntries([Argument] DateOnly start, [Argument] DateOnly end)
  {
    await _terminalService.ShowEntries(start, end);
  }

  [Command("create")]
  public async Task CreateEntry(
    [Option] DateTime start, 
    [Option] DateTime? end,
    [Option] string title,
    [Argument] string? content)
  {
    await _terminalService.CreateEntry(start, end, title, content);
  }

  [Command("update")]
  public async Task UpdateEntry(
    [Argument] Guid key,
    [Option] DateTime? start,
    [Option] DateTime? end,
    [Option] string? title,
    [Option] string? content)
  {
    await _terminalService.UpdateEntry(key, start, end, title, content);
  }

  [Command("delete")]
  public async Task DeleteEntry([Argument] Guid key)
  {
    await _terminalService.DeleteEntry(key);
  }
}