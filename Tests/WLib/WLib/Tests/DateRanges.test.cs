namespace WLibTests.Tests;

public class DateRanges
{
  private DateTimeRangeEnumerator _en = null!;

  [SetUp]
  public void Setup()
  {
    _en = new DateTimeRangeEnumerator(
      new DateOnly(2024, 1, 1),
      new DateOnly(2024, 1, 14),
      TimeSpan.FromHours(2),
      new TimeOnly(8, 0),
      new TimeOnly(16, 0),
      skipWeekends: true);
  }

  [Test]
  public void ShouldFindAllTimeSlots()
  {
    var result = _en.ToArray();
    Assert.That(result.Length, Is.EqualTo(40), "There should be 40 (8 hours per day / 2 hous * 10 working days) time slots but there were {0}.", result.Length);
  }
}