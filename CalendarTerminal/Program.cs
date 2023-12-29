
using CalendarTerminal;
using Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repositories;
using Repositories.Context;

var configuration = new ConfigurationBuilder()
  .SetBasePath(Directory.GetCurrentDirectory())
  .AddJsonFile("appsettings.json", optional: false)
  .Build();

var settings = new AppSettings();
configuration.Bind(settings);

var host = Host.CreateDefaultBuilder()
  .ConfigureServices((context, services) =>
  {
    services.AddDbContextPool<CalendarDbContext>(options =>
      options
        .UseMySql(settings.Database.ConnectionString, new MariaDbServerVersion(new Version(10, 6, 5)))
    );
    services.AddMyCalendar<CalendarRepository>();
    services.AddScoped<CalendarRepository>();
    services.AddSingleton<TerminalService>();
  })
  .Build();

if (settings.Database.Initialize)
{
  var db = host.Services.GetService<CalendarDbContext>();
  if (db is null)
  {
    return -1;
  }

  // Delete any and everything there is and recreate.
  // There is no migrations in this project.
  db.Database.EnsureDeleted();
  db.Database.EnsureCreated();
}

var service = host.Services.GetService<TerminalService>();
return service == null ? -1 : await service.Run();