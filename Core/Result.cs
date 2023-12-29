namespace Core;

public abstract class ResultBase
{
  protected ResultBase(Exception? ex)
  {
    Error = ex;
  }

  protected Exception? Error { get; private set; }

  public void OnSuccess(Action action)
  {
    if (Error == null)
    {
      action();
    }
  }

  public void OnError(Action<Exception> action)
  {
    if (Error != null)
    {
      action(Error);
    }
  }
}

public class Result : ResultBase
{
  private Result(Exception? error = null)
    : base(error)
  {
  }

  internal static async Task<Result> Create(Func<Task> action)
  {
    try
    {
      await action();
      return new Result();
    }
    catch (Exception ex)
    {
      return new Result(ex);
    }
  }

  public static Result Ok() => new Result(null);
  public static Result Fail(Exception ex) => new Result(ex);
  public static Result Fail(string msg) => new Result(new Exception(msg));
}

public abstract class Result<TSuccess> : ResultBase
{
  protected Result(Exception? ex)
    : base(ex) { }

  protected TSuccess? Success { private get; set; }

  public void OnSuccess(Action<TSuccess> action)
  {
    if (Success != null)
    {
      action(Success);
    }
  }
};
