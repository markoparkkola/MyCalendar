using System.Globalization;

namespace WLibTests.Tests;

public class Weekends
{
  private Weekend _weekend;

  [OneTimeSetUp]
  public void OneTimeSetUp()
  {
    _weekend = new Weekend(CultureInfo.InvariantCulture.Calendar);
  }

  [TestCaseSource(nameof(DaysOfSomeWeek))]
  public void ShouldCheckWeekendsRight(KeyValuePair<DateTime, bool> kvp)
  {
    Assert.That(_weekend.IsWeekend(kvp.Key), Is.EqualTo(kvp.Value));
  }

  public static KeyValuePair<DateTime, bool>[] DaysOfSomeWeek =
  {
    new KeyValuePair<DateTime, bool>(new DateTime(2024, 1, 1), false),
    new KeyValuePair<DateTime, bool>(new DateTime(2024, 1, 2), false),
    new KeyValuePair<DateTime, bool>(new DateTime(2024, 1, 3), false),
    new KeyValuePair<DateTime, bool>(new DateTime(2024, 1, 4), false),
    new KeyValuePair<DateTime, bool>(new DateTime(2024, 1, 5), false),
    new KeyValuePair<DateTime, bool>(new DateTime(2024, 1, 6), true),
    new KeyValuePair<DateTime, bool>(new DateTime(2024, 1, 7), true),
  };
}

