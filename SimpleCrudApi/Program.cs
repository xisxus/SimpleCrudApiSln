using Microsoft.EntityFrameworkCore;
using SimpleCrudApi.Models;
using SimpleCrudApi.Controllers;
using SimpleCrudApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddDbContext<AppDbContext>(op=> op.UseSqlServer(builder.Configuration.GetConnectionString("connection")));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IImageHandleService, ImageHandlerService>();




var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();



app.Run();
