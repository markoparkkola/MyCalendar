using Core.Models;

namespace Calendar.Models;

public class CalendarEntryJsonModel
{
  public Guid Key { get; init; }
  public DateTime Start {  get; init; }
  public DateTime? End { get; init; }
  public string Title { get; init; } = string.Empty;
  public string Content { get; init; } = string.Empty;

  public static explicit operator CalendarEntryJsonModel(CalendarEntry entry)
    => new CalendarEntryJsonModel
    {
      Key = entry.Key,
      Start = entry.Date.Start,
      End = entry.Date.End,
      Title = entry.Title,
      Content = entry.Content,
    };
}
