using Core.Models;
using Core.Services;
using System.Collections.Immutable;

namespace CalendarTerminal;

internal class TerminalService
{
  private readonly ICalendarService _calendarService;

  public TerminalService(ICalendarService calendarService)
  {
    _calendarService = calendarService;
  }

  public async Task ShowEntries(DateOnly? start, DateOnly? end)
  {
    GetCalendarEntryFilters filters =
      start is null && end is null ?
        GetCalendarEntryFilters.Empty :
        new GetCalendarEntryFilters(start, end);

    var result = await _calendarService.GetCalendarEntriesAsync(filters);
    result.OnSuccess((entries) =>
    {
      var orderedEntries = entries.OrderBy(e => e.Date).ToImmutableList();

      Console.WriteLine();
      Console.WriteLine("*** Entries ***");
      Console.WriteLine();
      Console.WriteLine("-----");

      foreach (var entry in orderedEntries)
      {
        Console.WriteLine(entry);
        Console.WriteLine("-----");
      }

      Console.WriteLine();
    });
  }

  public async Task CreateEntry(DateTime start, DateTime? end, string title, string? content)
  {
    var isWholeDay = end is null;

    var result = await _calendarService.StoreCalendarEntryAsync(
      new NewCalendarEntry(
        new CalendarDate(end is null ? start.Date : start, end),
        title, 
        content ?? ""
        )
      );
    result.OnSuccess(() => Console.WriteLine("Created."));
  }

  public async Task UpdateEntry(Guid key, DateTime? start, DateTime? end, string? title, string? content)
  {
    var result = await _calendarService.GetCalendarEntryAsync(key);
    result.OnSuccess(async (entry) =>
    {
      DateTime startDate = start is null ?
            entry.Date.Start :
            end is null ?
              start.Value.Date :
              start.Value;

      var updateResult = await _calendarService.UpdateCalendarEntryAsync(
        new UpdateCalendarEntry(key,
          new CalendarDate(startDate, end),
          title ?? entry.Title,
          content ?? entry.Content
          )
        );

      updateResult
        .OnSuccess(() => Console.WriteLine("Updated."))
        .OnError((e) => Console.WriteLine(e.Message));
    })
    .OnError((e) => Console.WriteLine(e.Message));
  }

  public async Task DeleteEntry(Guid key)
  {
    var result = await _calendarService.RemoveCalendarEntryAsync(key);
    result.OnSuccess(() => Console.WriteLine("Removed."));
  }
}
