using Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Entities;

public class CalendarEntry
{
  [Key]
  public Guid Key { get; init; }
  public DateTime Start {  get; set; }
  public DateTime? End { get; set; }
  public string Title { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public static explicit operator Core.Models.CalendarEntry(CalendarEntry entry)
    => new Core.Models.CalendarEntry(entry.Key, new CalendarDate(entry.Start, entry.End), entry.Title, entry.Content);
}
