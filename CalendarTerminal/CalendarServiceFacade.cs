using Core.Models;
using Core.Services;
using System.Collections.Immutable;
using WLib;

namespace CalendarTerminal;

/// <summary>
/// Just a facade or wrapper to demonstrate the usage of Result class.
/// The app would work perfectly fine without this also.
/// </summary>
public class CalendarServiceFacade
{
  private readonly ICalendarService _calendarService;

  public CalendarServiceFacade(ICalendarService calendarService)
  {
    _calendarService = calendarService;
  }

  internal async Task<CalendarServiceResult<IImmutableSet<CalendarEntry>>> GetCalendarEntriesAsync(GetCalendarEntryFilters filters)
  {
    return await CalendarServiceResult<IImmutableSet<CalendarEntry>>.Create(() => _calendarService.GetCalendarEntriesAsync(filters));
  }

  internal async Task<CalendarServiceResult<CalendarEntry>> GetCalendarEntryAsync(Guid key)
  {
    return await CalendarServiceResult<CalendarEntry>.Create(() => _calendarService.GetCalendarEntryAsync(key));
  }

  internal async Task<Result> RemoveCalendarEntryAsync(Guid key)
  {
    return await Result.Create(() => _calendarService.RemoveCalendarEntryAsync(key));
  }

  internal async Task<Result> StoreCalendarEntryAsync(NewCalendarEntry newCalendarEntry)
  {
    return await Result.Create(() => _calendarService.StoreCalendarEntryAsync(newCalendarEntry));
  }

  internal async Task<CalendarServiceResult<DateTime?>> SuggestCalendarEntryAsync(DateOnly start, DateOnly end, TimeSpan length, TimeOnly startTime, TimeOnly endTime, bool skipWeekends)
  {
    return await CalendarServiceResult<DateTime?>.Create(() => _calendarService.SuggestCalendarEntryAsync(start, end, length, startTime, endTime, skipWeekends));
  }

  internal async Task<Result> UpdateCalendarEntryAsync(UpdateCalendarEntry updateCalendarEntry)
  {
    return await Result.Create(() => _calendarService.UpdateCalendarEntryAsync(updateCalendarEntry));
  }
}
