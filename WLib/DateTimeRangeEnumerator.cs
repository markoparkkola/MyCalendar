using System.Collections;
using System.Globalization;

namespace WLib;

/// <summary>
/// Enumerates date times through given range and finds all the possible time slots with defined length.
/// </summary>
public class DateTimeRangeEnumerator : IEnumerator<DateTime>, IEnumerable<DateTime>
{
  class DateTimeSlider
  {
    private DateTime _value;
    private readonly DateTime _end;

    public DateTimeSlider(DateTime initialValue, DateTime end)
    {
      _value = initialValue;
      _end = end;
    }

    public bool IsValid => _value < _end;

    public DateTime Current => _value;

    public bool IsWeekend(Weekend weekend)
      => weekend.IsWeekend(_value);

    public void AddDaysAndTime(int days, TimeOnly time)
    {
      _value = _value.AddDays(days).Date + time.ToTimeSpan();
    }

    public bool IsValidWhile(TimeSpan length, TimeOnly endTimeOfDay)
    {
      var endTime = _value.Add(length);
      return TimeOnly.FromTimeSpan(endTime.TimeOfDay) <= endTimeOfDay;
    }
  }

  private readonly DateOnly _start;
  private readonly DateOnly _end;
  private readonly TimeSpan _length;
  private readonly TimeOnly _startTime;
  private readonly TimeOnly _endTime;
  private readonly bool _skipWeekends;
  private readonly Weekend _weekend;

  /// <summary>
  /// Constructor.
  /// </summary>
  /// <param name="start">Start date of range.</param>
  /// <param name="end">End date of range.</param>
  /// <param name="length">Length of desired slot.</param>
  /// <param name="startTime">Start time of day. Time slots are not found before this time of day.</param>
  /// <param name="endTime">End time of day.  Time slots are not found after this time of day.</param>
  /// <param name="skipWeekends">If true, skips Saturdays and Sundays.</param>
  public DateTimeRangeEnumerator(
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
    _weekend = new Weekend(CultureInfo.CurrentCulture.Calendar);
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
    var end = _end.ToDateTime(_endTime);
    while (start < end)
    {
      if (_skipWeekends && _weekend.IsWeekend(start))
      {
        start = start.Date.AddDays(1).Date + _startTime.ToTimeSpan();
        continue;
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
