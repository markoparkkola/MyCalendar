
namespace WLib;

public record DateTimeRange(DateTime start, DateTime end)
{
  public bool ConflictsWith(DateTime time)
    => time >= start && time <= end;
}
