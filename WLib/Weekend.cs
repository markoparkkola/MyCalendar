using System.Globalization;

namespace WLib;

public class Weekend
{
  private readonly Calendar _calendar;
  private readonly HashSet<DayOfWeek> _days;

  public Weekend(Calendar calendar, HashSet<DayOfWeek>? days = null)
  {
    _calendar = calendar;
    _days = days ?? new[]
    {
      DayOfWeek.Saturday,
      DayOfWeek.Sunday
    }.ToHashSet();
  }

  public bool IsWeekend(DateTime date) => _days.Contains(_calendar.GetDayOfWeek(date));
  public bool IsWeekend(DateOnly date) => IsWeekend(date.ToDateTime(TimeOnly.MinValue));
}
