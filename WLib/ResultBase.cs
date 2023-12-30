namespace WLib;

/// <summary>
/// <para>
/// Simple, abstract result class base. Instead of returning arbitrary values from functions
/// and throwing exceptions when things go wrong, you can return some meaningful result
/// from the function. Caller then calls <see cref="OnSuccess(Action)"/> to act when
/// the result is success and optionaly handle errors in <see cref="OnError(Action{Exception})" />.
/// </para>
/// </summary>
public abstract class ResultBase
{
  protected ResultBase(Exception? ex)
  {
    Error = ex;
  }

  protected Exception? Error { get; private set; }

  /// <summary>
  /// Call to handle successful result.
  /// </summary>
  /// /// <returns>This.</returns>
  public ResultBase OnSuccess(Action action)
  {
    if (Error == null)
    {
      action();
    }
    return this;
  }

  /// <summary>
  /// Call to handle errors.
  /// </summary>
  /// /// <returns>This.</returns>
  public ResultBase OnError(Action<Exception> action)
  {
    if (Error != null)
    {
      action(Error);
    }
    return this;
  }
}
