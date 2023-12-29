namespace Core.Services;

public class CalendarServiceResult<TResult> : Result<TResult, Exception>
{
  private readonly TResult? _success;
  private readonly Exception? _error;
  private readonly bool _isSuccess;

  private CalendarServiceResult(bool isSuccess, TResult? success, Exception? error)
  {
    _success = success;
    _error = error;
    _isSuccess = isSuccess;
  }

  protected override TResult? Success => _success;
  protected override Exception? Error => _error;
  protected override bool IsSuccess => _isSuccess;

  internal static async Task<CalendarServiceResult<TResult>> Create(Func<Task<TResult>> task)
  {
    try
    {
      var result = await task();
      return new CalendarServiceResult<TResult>(true, result, null);
    }
    catch (Exception exception)
    {
      return new CalendarServiceResult<TResult>(false, default, exception);
    }
  }
}
