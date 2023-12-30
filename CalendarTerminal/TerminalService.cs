using Core.Models;
using Core.Services;
using System.Collections.Immutable;

namespace CalendarTerminal;

public class TerminalService
{
  private readonly CalendarServiceFacade _calendarService;

  public TerminalService(CalendarServiceFacade calendarService)
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

  public async Task CreateEntry(DateTime start, DateTime end, bool isFullDay, string title, string? content)
  {
    var result = await _calendarService.StoreCalendarEntryAsync(
      new NewCalendarEntry(
        new CalendarDate(
          isFullDay ? start.Date : start, 
          isFullDay ? end.Date : end,
          isFullDay),
        title, 
        content ?? ""
        )
      );
    result.OnSuccess(() => Console.WriteLine("Created."));
  }

  public async Task UpdateEntry(Guid key, DateTime? start, DateTime? end, bool? isFullDay, string? title, string? content)
  {
    var result = await _calendarService.GetCalendarEntryAsync(key);
    result.OnSuccess(async (entry) =>
    {
      var updateResult = await _calendarService.UpdateCalendarEntryAsync(
        new UpdateCalendarEntry(key,
          new CalendarDate(
            ResolveDate(start, entry.Date.Start, isFullDay ?? entry.Date.IsFullDay),
            ResolveDate(end, entry.Date.End, isFullDay ?? entry.Date.IsFullDay),
            isFullDay ?? entry.Date.IsFullDay),
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

  internal async Task SuggestEntry(DateOnly start, DateOnly end, TimeSpan length, TimeOnly startTime, TimeOnly endTime, bool skipWeekends)
  {
    var result = await _calendarService.SuggestCalendarEntryAsync(start, end, length, startTime, endTime, skipWeekends);
    result.OnSuccess((time) => Console.WriteLine((object?)time ?? "No suggestion."));
  }

  private static DateTime ResolveDate(DateTime? date, DateTime @default, bool isFullDay)
  {
    static DateTime GetDate(DateTime date, bool isFullDay)
      => isFullDay ? date.Date : date;

    return GetDate(date ?? @default, isFullDay);
  }
}
