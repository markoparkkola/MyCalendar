namespace WLibTests.Tests;

public class DateTimeRange
{
  [TestCaseSource(nameof(DateTimeRanges))]
  public void TestConflicts((DateTime start, DateTime end, DateTime value, bool conflicts) args)
  {
    var range = new WLib.DateTimeRange(args.start, args.end);
    Assert.That(range.ConflictsWith(args.value), Is.EqualTo(args.conflicts));
  }

  public static (DateTime start, DateTime end, DateTime value, bool conflicts)[] DateTimeRanges = new[]
  {
    (new  DateTime(2014, 1, 1), new  DateTime(2014, 1, 2), new DateTime(2014, 1, 1), true),
    (new  DateTime(2014, 1, 1), new  DateTime(2014, 1, 2), new DateTime(2014, 1, 3), false),
    (new  DateTime(2014, 1, 1, 12, 0, 0), new  DateTime(2014, 1, 1, 13, 0, 0), new DateTime(2014, 1, 1, 12, 30, 0), true),
    (new  DateTime(2014, 1, 1, 12, 0, 0), new  DateTime(2014, 1, 1, 13, 0, 0), new DateTime(2014, 1, 1, 13, 30, 0), false),
    (new  DateTime(2014, 1, 1, 12, 0, 0), new  DateTime(2014, 1, 1, 13, 0, 0), new DateTime(2014, 1, 1, 13, 0, 0), false),
  };
}
