namespace Core.Services;

/// <summary>
/// Filters that are used in WHERE clause of the SQL query. It is many times more clear to gather 
/// all the filters in one specific class than pass them around as parameters.
/// </summary>
public record GetCalendarEntryFilters(DateOnly? Start, DateOnly? End)
{
  private static readonly GetCalendarEntryFilters _empty  = new GetCalendarEntryFilters(null, null);

  public static GetCalendarEntryFilters Empty { get => _empty; }
}