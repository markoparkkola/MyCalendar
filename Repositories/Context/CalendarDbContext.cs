using Microsoft.EntityFrameworkCore;
using Repositories.Entities;

namespace Repositories.Context;

public class CalendarDbContext : DbContext
{
    public CalendarDbContext(DbContextOptions<CalendarDbContext> options)
      : base(options)
    {

    }

    public virtual DbSet<CalendarEntry> CalendarEntries { get; set; }
}