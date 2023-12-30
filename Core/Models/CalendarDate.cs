namespace Core.Models;

public record CalendarDate(DateTime Start, DateTime End, bool IsFullDay) : IComparable<CalendarDate>
{
  public override string ToString()
  {
    if (IsFullDay)
    {
      return $"{DateOnly.FromDateTime(Start)} - {DateOnly.FromDateTime(End)}";
    }
    return $"{Start} - {End}";
  }

  int IComparable<CalendarDate>.CompareTo(CalendarDate? other)
    => other?.Start.CompareTo(Start) ?? 0;
}