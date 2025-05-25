using Microsoft.EntityFrameworkCore;
using SimpleBlog.API.Configuration;
using SimpleBlog.Infrastructure.Data;
using SimpleBlog.Infrastructure.Middleware;
using SimpleBlog.Infrastructure.WebSockets;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProjectDependencies();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<SimpleBlogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseWebSockets();

app.Map("/ws/notifications", async context =>
{
    var handler = context.RequestServices.GetRequiredService<NotificationWebSocketHandler>();
    await handler.HandleAsync(context);
});

app.Run();
