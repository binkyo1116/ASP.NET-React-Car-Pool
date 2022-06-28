namespace Carpool.WebUI.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTTokenKey"]));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidateAudience = false
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                  {
                      var accessToken = context.Request.Query["access_token"];
                      var path = context.HttpContext.Request.Path;
                      if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/chat")))
                      {
                          context.Token = accessToken;
                      }
                      return System.Threading.Tasks.Task.CompletedTask;
                  }
            };
        });

        services.AddAuthorization();
        services.AddScoped<TokenService>();

        return services;
    }
}
