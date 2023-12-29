using Core;
using Core.Models;
using Core.Services;
using System.Collections.Immutable;
using static CalendarTerminal.Utils;

namespace CalendarTerminal;

internal class TerminalService
{
  private readonly ICalendarService _calendarService;

  public TerminalService(ICalendarService calendarService)
  {
    _calendarService = calendarService;
  }

  public async Task<int> Run()
  {
    ShowInit();

    string command;

    do
    {
      Console.Write("> ");
      var line = Console.ReadLine() ?? "quit";
      var pieces = line.Split(' ');
      command = pieces[0];

      var result = command switch
      {
        "list" => await ShowEntries(true),
        "find" => await ShowEntries(),
        "add" => await CreateEntry(),
        "del" => await DeleteEntry(),
        "update" => await UpdateEntry(),
        "help" => ShowHelp(),
        _ => Result.Fail(new NotSupportedException("Unknown command")),
      };

      result.OnError((e) => 
      {
        Console.WriteLine($"Error: {e}");
      });
      Console.WriteLine();

    } while (command != "quit");

    return 0;
  }

  private async Task<ResultBase> UpdateEntry()
  {
    Console.Write("Key: ");
    var key = GetGuidFromConsole();

    var result = await _calendarService.GetCalendarEntryAsync(key);
    result.OnSuccess(async (entry) =>
    {
      Console.WriteLine();

      Console.WriteLine($"Old value: {entry.Date.IsWholeDay}");
      Console.Write("Whole day (Y/N)? ");
      var isWholeDay = GetBooleanFromConsole(entry.Date.IsWholeDay);

      Console.WriteLine($"Old value: {entry.Date.Start}");
      Console.Write("Start time: ");
      var startTime = isWholeDay ?
        GetDateFromConsole(DateOnly.FromDateTime(entry.Date.Start)).ToDateTime(TimeOnly.MinValue) :
        GetDateTimeFromConsole(entry.Date.Start);

      DateTime? endTime = null;
      if (!isWholeDay)
      {
        Console.WriteLine($"Old value: {entry.Date.End}");
        Console.Write("End time: ");
        endTime = GetDateTimeFromConsole();
      }

      Console.WriteLine($"Old value: {entry.Title}");
      Console.Write("Title: ");
      var title = ReadLineFromConsole(entry.Title);

      Console.WriteLine($"Old value: {entry.Content}");
      Console.WriteLine("Content (end with double enter):");
      var content = GetMultiLineFromConsole(entry.Content);

      var updateResult = await _calendarService.UpdateCalendarEntryAsync(
        new UpdateCalendarEntry(key,
          new CalendarDate(startTime, endTime),
          title,
          content
          )
        );
      updateResult.OnSuccess(() => Console.WriteLine("Updated."));
    });

    return result;
  }

  private async Task<ResultBase> DeleteEntry()
  {
    Console.Write("Key: ");
    var key = GetGuidFromConsole();

    var result = await _calendarService.RemoveCalendarEntryAsync(key);
    result.OnSuccess(() => Console.WriteLine("Removed."));
    return result;
  }

  private async Task<ResultBase> CreateEntry()
  {
    Console.WriteLine();
    Console.Write("Whole day (Y/N)? ");
    var isWholeDay = GetBooleanFromConsole();

    Console.Write("Start time: ");
    var startTime = isWholeDay ? 
      GetDateFromConsole().ToDateTime(TimeOnly.MinValue) : 
      GetDateTimeFromConsole();

    DateTime? endTime = null;
    if (!isWholeDay)
    {
      Console.Write("End time: ");
      endTime = GetDateTimeFromConsole();
    }

    Console.Write("Title: ");
    var title = ReadLineFromConsole();

    Console.WriteLine("Content (end with double enter):");
    var content = GetMultiLineFromConsole();

    var result = await _calendarService.StoreCalendarEntryAsync(
      new NewCalendarEntry(
        new CalendarDate(startTime, endTime),
        title, 
        content
        )
      );
    result.OnSuccess(() => Console.WriteLine("Created."));
    return result;
  }

  private async Task<ResultBase> ShowEntries(bool skipFilters = false)
  {
    GetCalendarEntryFilters filters = GetCalendarEntryFilters.Empty;

    if (!skipFilters)
    {
      Console.Write("Start date: ");
      var startDate = GetDateFromConsole();

      Console.Write("End date: ");
      var endDate = GetDateFromConsole();

      filters = new GetCalendarEntryFilters(startDate, endDate);
    }

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

    return result;
  }

  private void ShowInit()
  {
    Console.WriteLine(
"""
MyCalendar console app.
Type help for help.

""");
  }

  private ResultBase ShowHelp()
  {
    Console.WriteLine(
"""
*** HELP ***

list
find
add
del
update
quit

Date format: 31.12.2023
Date time format: 31.12.2023 23:45
""");

    return Result.Ok();
  }
}
