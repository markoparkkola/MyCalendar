
namespace Core.Models;

public record CalendarEntry(Guid Key, CalendarDate Date, string Title, string Content) 
  : CalendarEntryWithoutKey(Date, Title, Content)
{
  public override string ToString() => 
$"""
Key: {Key}
{Date}
{Title}

{Content}
""";
}
