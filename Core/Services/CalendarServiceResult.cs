namespace Core.Services;

public class CalendarServiceResult<TResult> : Result<TResult>
{
  private CalendarServiceResult(TResult? result) 
    :base(null)
  {
    base.Success = result;
  }

  private CalendarServiceResult(Exception exception)
    :base(exception)
  {
  }

  internal static async new Task<CalendarServiceResult<TResult>> Success(Func<Task<TResult>> task)
  {
    try
    {
      var result = await task();
      return new CalendarServiceResult<TResult>(result);
    }
    catch (Exception exception)
    {
      return new CalendarServiceResult<TResult>(exception);
    }
  }
}
