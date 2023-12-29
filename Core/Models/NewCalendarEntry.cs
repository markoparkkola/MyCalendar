namespace Core.Models;

public record NewCalendarEntry(CalendarDate Date, string Title, string Content)
  : CalendarEntryWithoutKey(Date, Title, Content);
