using Microsoft.Extensions.DependencyInjection;

namespace Core.Services;

public static class MyCalendarExt
{
  public static IServiceCollection AddMyCalendar<TRepository>(this IServiceCollection services)
    where TRepository : ICalendarRepository
  {
    services.AddTransient<ICalendarService, CalendarService>();
    services.AddTransient<ICalendarRepository>((provider) => 
      provider.GetService<TRepository>() ?? throw new Exception("Repository was not configured."));
    return services;
  }
}
