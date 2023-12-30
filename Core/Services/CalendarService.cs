using Core.Models;
using System.Collections.Immutable;
using System.Threading.Tasks.Sources;
using WLib;

namespace Core.Services;

/// <summary>
/// Simple service layer to handle different calendar operations.
/// </summary>
public class CalendarService : ICalendarService
{
  private readonly ICalendarRepository _calendarRepository;

  /// <summary>
  /// Constructor.
  /// </summary>
  /// <param name="calendarRepository">Storage where entries are persisted.</param>
  public CalendarService(ICalendarRepository calendarRepository)
  {
    _calendarRepository = calendarRepository;
  }

  public async Task<IImmutableSet<CalendarEntry>> GetCalendarEntriesAsync(GetCalendarEntryFilters filters, CancellationToken cancellationToken = default)
    => await _calendarRepository.GetEntriesAsync(filters.Start, filters.End, cancellationToken);

  public async Task<CalendarEntry> GetCalendarEntryAsync(Guid key, CancellationToken cancellationToken = default)
    => await _calendarRepository.GetEntryAsync(key, cancellationToken);

  public async Task RemoveCalendarEntryAsync(Guid key, CancellationToken cancellationToken = default)
    => await _calendarRepository.DeleteEntryAsync(key, cancellationToken);

  public async Task StoreCalendarEntryAsync(NewCalendarEntry entry, CancellationToken cancellationToken = default)
    => await _calendarRepository.StoreEntryAsync(entry, cancellationToken);

  public async Task<DateTime?> SuggestCalendarEntryAsync(DateOnly start, DateOnly end, TimeSpan length, TimeOnly startTime, TimeOnly endTime, bool skipWeekends, CancellationToken cancellationToken = default)
  {
    var conflictingEntries = await _calendarRepository.GetEntriesAsync(start, end, cancellationToken);

    var conflictingRanges = conflictingEntries.Select(x => new DateTimeRange(x.Date.Start, x.Date.End)).ToImmutableList();

    var timeEnumerator = new DateTimeEnumerator(start, end, length, startTime, endTime, skipWeekends);
    foreach (var time in timeEnumerator)
    {
      if (conflictingRanges.All(x => !x.ConflictsWith(time)))
      {
        return time;
      }
    }

    return null;
  }

  public async Task UpdateCalendarEntryAsync(UpdateCalendarEntry entry, CancellationToken cancellationToken = default)
    => await _calendarRepository.UpdateEntryAsync(entry, cancellationToken);
}
