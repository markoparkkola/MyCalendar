namespace Core.Models;

public record CalendarDate(DateTime Start, DateTime? End) : IComparable<CalendarDate>
{
  public bool IsWholeDay { get => End is null; }

  public override string ToString()
    => IsWholeDay ? DateOnly.FromDateTime(Start).ToString() : $"{Start} - {End}";

  int IComparable<CalendarDate>.CompareTo(CalendarDate? other)
    => other?.Start.CompareTo(Start) ?? 0;
}