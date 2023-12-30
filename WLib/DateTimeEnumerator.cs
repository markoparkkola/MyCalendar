using System.Collections;
using System.Globalization;

namespace WLib;

public class DateTimeEnumerator : IEnumerator<DateTime>, IEnumerable<DateTime>
{
  private readonly DateOnly _start;
  private readonly DateOnly _end;
  private readonly TimeSpan _length;
  private readonly TimeOnly _startTime;
  private readonly TimeOnly _endTime;
  private readonly bool _skipWeekends;

  public DateTimeEnumerator(
    DateOnly start,
    DateOnly end,
    TimeSpan length,
    TimeOnly startTime,
    TimeOnly endTime,
    bool skipWeekends = true
  )
  {
    _start = start;
    _end = end;
    _length = length;
    _startTime = startTime;
    _endTime = endTime;
    _skipWeekends = skipWeekends;
  }

  private DateTime? _current;

  public DateTime Current => _current ?? throw new Exception("Time slot was not found.");

  object IEnumerator.Current => _current ?? throw new Exception("Time slot was not found.");

  public void Dispose()
  {
    // nothing to dispose
  }

  public bool MoveNext()
  {
    _current = GetNextTimeSlot(
      _current == null ?
        _start.ToDateTime(_startTime) :
        _current.Value.Add(_length)
    );
    return _current != null;
  }

  public void Reset()
  {
    _current = null;
  }

  private DateTime? GetNextTimeSlot(DateTime start)
  {
    var calendar = CultureInfo.CurrentCulture.Calendar;

    var end = _end.ToDateTime(_endTime);
    while (start < end)
    {
      if (_skipWeekends)
      {
        var dow = calendar.GetDayOfWeek(start);
        if (dow == DayOfWeek.Saturday || dow == DayOfWeek.Sunday)
        {
          start = start.Date.AddDays(1).Date + _startTime.ToTimeSpan();
          continue;
        }
      }

      var slotEndTime = start.Add(_length);
      if (TimeOnly.FromTimeSpan(slotEndTime.TimeOfDay) <= _endTime)
      {
        return start;
      }
      start = start.Date.AddDays(1).Date + _startTime.ToTimeSpan();
    }

    return null;
  }

  #region Enumerable
  public IEnumerator<DateTime> GetEnumerator()
  {
    return this;
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    return this;
  }
  #endregion
}
