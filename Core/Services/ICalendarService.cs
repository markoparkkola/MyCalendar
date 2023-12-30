using Core.Models;
using System.Collections.Immutable;
using WLib;

namespace Core.Services;

/// <summary>
/// Calendar service interface.
/// </summary>
/// <remarks>
/// Even though I could use <see cref="WLib.Result"/> or <see cref="WLib.Result{TSuccess, TError}"/> here,
/// those are not really suitable for Asp.Net (Core) controller. You'd need some ugly piece of code to handle
/// the results or return error status code to the client. Maybe I'll write some ControllerFactory middleware
/// to handle Result types.
/// </remarks>
public interface ICalendarService
{
  Task<IImmutableSet<CalendarEntry>> GetCalendarEntriesAsync(GetCalendarEntryFilters filters, CancellationToken cancellationToken = default);
  Task<CalendarEntry> GetCalendarEntryAsync(Guid key, CancellationToken cancellationToken = default);
  Task RemoveCalendarEntryAsync(Guid key, CancellationToken cancellationToken = default);
  Task StoreCalendarEntryAsync(NewCalendarEntry entry, CancellationToken cancellationToken = default);
  Task<DateTime?> SuggestCalendarEntryAsync(DateOnly start, DateOnly end, TimeSpan length, TimeOnly startTime, TimeOnly endTime, bool skipWeekends, CancellationToken cancellationToken = default);
  Task UpdateCalendarEntryAsync(UpdateCalendarEntry entry, CancellationToken cancellationToken = default);
}
