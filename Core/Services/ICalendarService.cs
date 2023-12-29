using Core.Models;
using System.Collections.Immutable;

namespace Core.Services;

public interface ICalendarService
{
  Task<CalendarServiceResult<IImmutableSet<CalendarEntry>>> GetCalendarEntriesAsync(GetCalendarEntryFilters filters, CancellationToken cancellationToken = default);
  Task<CalendarServiceResult<CalendarEntry>> GetCalendarEntryAsync(Guid key, CancellationToken cancellationToken = default);
  Task<Result> RemoveCalendarEntryAsync(Guid key, CancellationToken cancellationToken = default);
  Task<Result> StoreCalendarEntryAsync(NewCalendarEntry entry, CancellationToken cancellationToken = default);
  Task<Result> UpdateCalendarEntryAsync(UpdateCalendarEntry entry, CancellationToken cancellationToken = default);
}
