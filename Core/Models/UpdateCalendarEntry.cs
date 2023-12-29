namespace Core.Models;

public record UpdateCalendarEntry(Guid Key, CalendarDate Date, string Title, string Content) 
  : CalendarEntryWithoutKey(Date, Title, Content);
