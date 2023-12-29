using Core.Models;
using System.Collections.Immutable;

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

  public async Task<CalendarServiceResult<IImmutableSet<CalendarEntry>>> GetCalendarEntriesAsync(GetCalendarEntryFilters filters, CancellationToken cancellationToken = default)
    => await CalendarServiceResult<IImmutableSet<CalendarEntry>>.Create(() => _calendarRepository.GetEntriesAsync(filters.Start, filters.End, cancellationToken));

  public async Task<CalendarServiceResult<CalendarEntry>> GetCalendarEntryAsync(Guid key, CancellationToken cancellationToken = default)
    => await CalendarServiceResult<CalendarEntry>.Create(() => _calendarRepository.GetEntryAsync(key, cancellationToken));

  public async Task<Result> RemoveCalendarEntryAsync(Guid key, CancellationToken cancellationToken = default)
    => await Result.Create(() => _calendarRepository.DeleteEntryAsync(key, cancellationToken));

  public async Task<Result> StoreCalendarEntryAsync(NewCalendarEntry entry, CancellationToken cancellationToken = default)
    => await Result.Create(() => _calendarRepository.StoreEntryAsync(entry, cancellationToken));

  public async Task<Result> UpdateCalendarEntryAsync(UpdateCalendarEntry entry, CancellationToken cancellationToken = default)
    => await Result.Create(() => _calendarRepository.UpdateEntryAsync(entry, cancellationToken));
}
