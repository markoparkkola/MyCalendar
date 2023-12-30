using CalendarTerminal;
using Cocona;
using Core.Services;

public class TerminalCommands
{
  private readonly TerminalService _terminalService;

  public TerminalCommands(TerminalService terminalService)
  {
    _terminalService = terminalService;
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
    [Option] DateTime end,
    [Option] bool? isFullDay,
    [Option] string title,
    [Argument] string? content)
  {
    await _terminalService.CreateEntry(start, end, isFullDay ?? false, title, content);
  }

  [Command("update")]
  public async Task UpdateEntry(
    [Argument] Guid key,
    [Option] DateTime? start,
    [Option] DateTime? end,
    [Option] bool? isFullDay,
    [Option] string? title,
    [Option] string? content)
  {
    await _terminalService.UpdateEntry(key, start, end, isFullDay, title, content);
  }

  [Command("delete")]
  public async Task DeleteEntry([Argument] Guid key)
  {
    await _terminalService.DeleteEntry(key);
  }

  [Command("suggest")]
  public async Task SuggestTime(
    [Argument] DateOnly start, 
    [Argument] DateOnly end,
    [Argument] TimeSpan length,
    [Option] TimeOnly? startTime,
    [Option] TimeOnly? endTime,
    [Option] bool? skipWeekends)
  {
    await _terminalService.SuggestEntry(start, end, length, startTime ?? new TimeOnly(8, 0), endTime ?? new TimeOnly(17, 0), skipWeekends ?? true);
  }
}