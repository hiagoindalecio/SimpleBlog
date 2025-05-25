using Microsoft.OpenApi.Models;

namespace SimpleBlog.API.Configuration
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SimpleBlog", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme.",
                });
                c.AddSecurityRequirement(
                  new OpenApiSecurityRequirement {
                    {
                      new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                          Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                      },
                      Array.Empty <string>()
                    }
                });
            });

            return services;
        }
    }
}
