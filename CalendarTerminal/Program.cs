using CalendarTerminal;
using Cocona;
using Core;
using Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Repositories;
using Repositories.Context;

var appBuilder = CoconaApp.CreateBuilder();

appBuilder.Logging.AddFilter("Microsoft.EntityFramework", LogLevel.Error);

appBuilder.Configuration
  .SetBasePath(Directory.GetCurrentDirectory())
  .AddJsonFile("appsettings.json", optional: false);

var settings = new AppSettings();
appBuilder.Configuration.Bind(settings);

appBuilder.Services.AddDbContextPool<CalendarDbContext>(options =>
  options
    .UseMySql(settings.Database.ConnectionString, new MariaDbServerVersion(new Version(10, 6, 5)))
);
appBuilder.Services.AddScoped<ICalendarRepository, CalendarRepository>();
appBuilder.Services.AddScoped<ICalendarService, CalendarService>();
appBuilder.Services.AddSingleton<CalendarServiceFacade>();
appBuilder.Services.AddSingleton<TerminalService>();

var host = appBuilder.Build();

if (settings.Database.Initialize)
{
  var db = host.Services.GetService<CalendarDbContext>() ?? throw new Exception("Database context was not configured.");

  // Delete any and everything there is and recreate.
  // There is no migrations in this project.
  db.Database.EnsureDeleted();
  db.Database.EnsureCreated();
}

host.AddCommands<TerminalCommands>();
host.Run();