namespace Core.Services;

public record GetCalendarEntryFilters(DateOnly? Start, DateOnly? End)
{
  private static readonly GetCalendarEntryFilters _empty  = new GetCalendarEntryFilters(null, null);

  public static GetCalendarEntryFilters Empty { get => _empty; }
}