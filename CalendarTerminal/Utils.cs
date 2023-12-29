using System.Text;

namespace CalendarTerminal;

public static class Utils
{
  public const string DateFormat = "d.M.yyyy";
  public const string DateTimeFormat = "d.M.yyyy hh:mm";

  public static bool GetBooleanFromConsole(bool? @default = null)
  {
    do
    {
      var key = Console.ReadKey();

      if (key.Key == ConsoleKey.Enter && @default is not null)
      {
        Console.WriteLine();
        return @default.Value;
      }

      if (key.KeyChar == 'Y' || key.KeyChar == 'y')
      {
        Console.WriteLine();
        return true;
      }

      if (key.KeyChar == 'N' || key.KeyChar == 'n')
      {
        Console.WriteLine();
        return false;
      }

    } while (true);
  }

  public static DateOnly GetDateFromConsole(DateOnly? @default = null)
  {
    do
    {
      var line = Console.ReadLine() ?? string.Empty;
      if (line.Length == 0 && @default is not null)
        return @default.Value;
      if (DateOnly.TryParseExact(line, DateFormat, null, System.Globalization.DateTimeStyles.None, out var result))
        return result;
    } while (true);
  }

  public static DateTime GetDateTimeFromConsole(DateTime? @default = null)
  {
    do
    {
      var line = Console.ReadLine() ?? string.Empty;
      if (line.Length == 0 && @default is not null)
        return @default.Value;
      if (DateTime.TryParseExact(line, DateTimeFormat, null, System.Globalization.DateTimeStyles.None, out var result))
        return result;
    } while (true);
  }

  public static string ReadLineFromConsole(string? @default = null)
  {
    do
    {
      var line = Console.ReadLine() ?? string.Empty;
      if (line.Length == 0 && @default is not null)
        return @default;
      if (line?.Length > 0)
        return line;
    } while (true);
  }

  public static string GetMultiLineFromConsole(string? @default = null)
  {
    var lastLine = string.Empty;
    var result = new StringBuilder();

    do
    {
      var line = Console.ReadLine() ?? string.Empty;
      if (line.Length == 0 && @default is not null)
        return @default;

      @default = null;
      if (string.IsNullOrEmpty(lastLine) && string.IsNullOrEmpty(line))
        return result.ToString();
      result.AppendLine(line);
      lastLine = line;
    } while (true);
  }

  public static Guid GetGuidFromConsole()
  {
    do
    {
      var line = Console.ReadLine() ?? string.Empty;
      if (Guid.TryParse(line, out var result)) 
        return result;
    } while (true);
  }
}
