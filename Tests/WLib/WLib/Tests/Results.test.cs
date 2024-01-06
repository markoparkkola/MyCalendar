namespace WLibTests.Tests;

public class Results
{
  [Test]
  public async Task ShouldReturnValueOnSuccess()
  {
    var result = await Result.Create(() => Task.FromResult(true));
    var actual = GetFinalResult(result, out var _);

    Assert.IsTrue(actual);
  }

  [Test]
  public async Task ShouldReturnErrorOnFailure()
  {
    const string failMessage = "Fail.";
    var result = await Result.Create(() => throw new Exception(failMessage));
    var actual = GetFinalResult(result, out var actualMessage);

    Assert.IsFalse(actual);
    Assert.That(actualMessage, Is.EqualTo(failMessage));
  }

  public static bool GetFinalResult(Result result, out string message)
  {
    bool success = false;
    Exception? exception = null;

    result.OnSuccess(() => success = true).OnError((ex) =>
    {
      success = false;
      exception = ex;
    });

    message = exception?.Message ?? string.Empty;
    return success;
  }
}
