using Microsoft.IdentityModel.Tokens;
using SimpleBlog.Application.Interfaces;
using SimpleBlog.Infrastructure.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SimpleBlog.API.Configuration
{
    public static class WebSocketsAppExtensions
    {
        public static IApplicationBuilder UseNotificationWebSocket(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/ws/notifications")
                {
                    var token = context.Request.Query["access_token"].ToString();

                    if (string.IsNullOrEmpty(token))
                    {
                        context.Response.StatusCode = 401;
                        return;
                    }

                    var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? new();
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var validationParams = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        ValidateLifetime = true
                    };

                    try
                    {
                        var principal = tokenHandler.ValidateToken(token, validationParams, out _);
                        context.User = principal;
                    }
                    catch
                    {
                        context.Response.StatusCode = 401;
                        return;
                    }

                    var handler = context.RequestServices.GetRequiredService<INotificationWebSocketHandler>();
                    await handler.HandleAsync(context);
                }
                else
                {
                    await next();
                }
            });

            return app;
        }
    }
}
