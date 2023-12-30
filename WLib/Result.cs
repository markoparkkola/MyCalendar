namespace WLib;

/// <summary>
/// Implementation of simple result class. <see cref="ResultBase"/> for more information.
/// </summary>
public class Result : ResultBase
{
  private Result(Exception? error = null)
    : base(error)
  {
  }

  /// <summary>
  /// Call to resolve the results and store them for the caller.
  /// </summary>
  /// <param name="action">The action to run.</param>
  public static async Task<Result> Create(Func<Task> action)
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
}

/// <summary>
/// Implementation or strongly typed result class.
/// </summary>
/// <typeparam name="TSuccess">Type of object that is returned when the action is succesful and 
/// user calls <see cref="OnSuccess(Action{TSuccess})"/> method.</typeparam>
/// <typeparam name="TError">Type of error object.</typeparam>
public abstract class Result<TSuccess, TError>
{
  protected abstract bool IsSuccess { get; }
  protected abstract TSuccess? Success { get; }
  protected abstract TError? Error { get; }

  /// <summary>
  /// Call to handle successful result.
  /// </summary>
  /// <param name="action">Callback to get and handle the results.</param>
  /// <returns>This.</returns>
  public Result<TSuccess, TError> OnSuccess(Action<TSuccess> action)
  {
    if (IsSuccess && Success is not null)
    {
      action(Success);
    }
    return this;
  }

  /// <summary>
  /// Call to handle errors.
  /// </summary>
  /// <param name="action">Callback to get the error object.</param>
  /// <returns>This.</returns>
  public Result<TSuccess, TError> OnError(Action<TError> action)
  {
    if (!IsSuccess && Error is not null)
    {
      action(Error);
    }
    return this;
  }
};
