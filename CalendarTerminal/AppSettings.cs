namespace CalendarTerminal;

public class AppSettings
{
  public class DatabaseSettings
  {
    public string ConnectionString { get; set; } = string.Empty;
    public bool Initialize { get; set; }
  }

  public DatabaseSettings Database { get; set; } = null!;
}
