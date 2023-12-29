using Core.Models;
using System.Collections.Immutable;

namespace Core;

public interface ICalendarRepository
{
  Task<CalendarEntry> GetEntryAsync(Guid key, CancellationToken cancellationToken = default);
  Task<IImmutableSet<CalendarEntry>> GetEntriesAsync(DateOnly? start, DateOnly? end, CancellationToken cancellationToken = default);
  Task StoreEntryAsync(NewCalendarEntry entry, CancellationToken cancellationToken = default);
  Task UpdateEntryAsync(UpdateCalendarEntry entry, CancellationToken cancellationToken = default);
  Task DeleteEntryAsync(Guid key, CancellationToken cancellationToken = default);
}