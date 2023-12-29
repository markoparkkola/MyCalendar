using Microsoft.EntityFrameworkCore;
using Core.Services;
using Repositories;
using Repositories.Context;
using Core;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("calendardb");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextPool<CalendarDbContext>(options =>
      options
        .UseMySql(connectionString, new MariaDbServerVersion(new Version(10, 6, 5)))
    );

builder.Services.AddTransient<CalendarRepository>();
builder.Services.AddMyCalendar<CalendarRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
