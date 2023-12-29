using Core;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Repositories.Context;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace Repositories;

public class CalendarRepository : ICalendarRepository
{
  private readonly CalendarDbContext _dbContext;

  public CalendarRepository(CalendarDbContext dbContext)
  {
    _dbContext = dbContext;
    _dbContext.Database.EnsureCreated();
  }

  public async Task DeleteEntryAsync(Guid key, CancellationToken cancellationToken = default)
  { 
    await _dbContext.CalendarEntries.Where(x => x.Key == key).ExecuteDeleteAsync(cancellationToken);
  }

  public async Task<IImmutableSet<CalendarEntry>> GetEntriesAsync(DateOnly? start, DateOnly? end, CancellationToken cancellationToken = default)
  {
    var startTime = start?.ToDateTime(TimeOnly.MinValue);
    var endTime = end?.ToDateTime(TimeOnly.MaxValue);

    var entriesInDb = await _dbContext.CalendarEntries
      .Where(x => startTime == null || x.Start >= startTime)
      .Where(x => endTime == null || x.End <= endTime)
      .ToListAsync(cancellationToken);

    return entriesInDb.Select(x => (CalendarEntry)x).ToImmutableHashSet();
  }

  public async Task<CalendarEntry> GetEntryAsync(Guid key, CancellationToken cancellationToken = default)
  {
    var entryInDb = await _dbContext.CalendarEntries.FindAsync(new object[] { key }, cancellationToken)
      ?? throw new Exception("Entry was not found.");

    return (CalendarEntry)entryInDb;
  }

  public async Task StoreEntryAsync(NewCalendarEntry entry, CancellationToken cancellationToken = default)
  {
    await _dbContext.CalendarEntries.AddAsync(new Entities.CalendarEntry
    {
      Key = Guid.NewGuid(),
      Start = entry.Date.Start,
      End = entry.Date.End,
      Title = entry.Title,
      Content = entry.Content
    }, cancellationToken);
    await _dbContext.SaveChangesAsync(cancellationToken);
  }

  public async Task UpdateEntryAsync(UpdateCalendarEntry entry, CancellationToken cancellationToken = default)
  {
    var entityIdDb = await _dbContext.CalendarEntries.FindAsync(new object[] { entry.Key, }, cancellationToken)
      ?? throw new Exception("Entry was not found.");

    entityIdDb.Start = entry.Date.Start;
    entityIdDb.End = entry.Date.End;
    entityIdDb.Title = entry.Title;
    entityIdDb.Content = entry.Content;

    _dbContext.Update(entityIdDb);
    await _dbContext.SaveChangesAsync(cancellationToken);
  }
}