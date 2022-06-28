namespace Carpool.WebUI.Extensions;

public static class OpenApiServiceExtensions
{
    public static IServiceCollection AddOpenApiServices(this IServiceCollection services)
    {
        services.AddOpenApiDocument(configure =>
       {
           configure.Title = "Carpool API";
           configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
           {
               Type = OpenApiSecuritySchemeType.ApiKey,
               Name = "Authorization",
               In = OpenApiSecurityApiKeyLocation.Header,
               Description = "Type into the textbox: Bearer {your JWT token}."
           });

           configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
       });
        return services;
    }
}
