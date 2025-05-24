using SimpleBlog.API.Configuration;
using SimpleBlog.Infrastructure.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProjectDependencies();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/ws")
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            //await webSocketHandler.HandleAsync(webSocket); // lógica de notificação
        }
    }
    else
    {
        await next();
    }
});

app.Run();
