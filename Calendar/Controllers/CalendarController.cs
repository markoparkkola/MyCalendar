using Calendar.Models;
using Core.Models;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Calendar.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class CalendarController : ControllerBase
  {
    private readonly ICalendarService _calendarService;

    public CalendarController(ICalendarService calendarService)
    {
      _calendarService = calendarService;
    }

    [HttpGet]
    public async Task<IEnumerable<CalendarEntryJsonModel>> Get(DateTime from, DateTime to, CancellationToken cancellationToken)
    {
      var fromDate = DateOnly.FromDateTime(from);
      var toDate = DateOnly.FromDateTime(to);

      var entries = await _calendarService.GetCalendarEntriesAsync(
        new GetCalendarEntryFilters(fromDate, toDate),
        cancellationToken);

      var result = new List<CalendarEntryJsonModel>();
      entries.OnSuccess(e =>
      {
        result.AddRange(e.Select(x => (CalendarEntryJsonModel)x));
      });
      entries.OnError((_) =>
      {
        Response.StatusCode = 500;
      });

      return result;
    }
  }
}
