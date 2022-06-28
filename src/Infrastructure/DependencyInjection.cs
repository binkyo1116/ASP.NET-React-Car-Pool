namespace Carpool.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityCore<ApplicationUser>(opt =>
                {
                    opt.Password.RequiredLength = 8;
                    opt.Password.RequireLowercase = true;
                    opt.Password.RequireUppercase = true;
                    opt.Password.RequireDigit = true;
                    opt.SignIn.RequireConfirmedEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager<SignInManager<ApplicationUser>>()
                .AddDefaultTokenProviders();
        services.Configure<DataProtectionTokenProviderOptions>(opt =>
           opt.TokenLifespan = TimeSpan.FromHours(2));

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("Carpool"));
        }
        else
        {
            string connStr = String.Empty;

            // Depending on if in development or production, use either Heroku-provided
            // connection string, or development connection string from env var.
            if (!configuration.GetValue<bool>("UseProduction"))
            {
                // Use connection string from file.
                connStr = configuration.GetConnectionString("DefaultConnection");
            }
            else if (configuration.GetValue<bool>("UseProduction") || Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                // Use connection string provided at runtime by Heroku.
                var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL")!;

                // Parse connection URL to connection string for Npgsql
                connUrl = connUrl.Replace("postgres://", string.Empty);
                var pgUserPass = connUrl.Split("@")[0];
                var pgHostPortDb = connUrl.Split("@")[1];
                var pgHostPort = pgHostPortDb.Split("/")[0];
                var pgDb = pgHostPortDb.Split("/")[1];
                var pgUser = pgUserPass.Split(":")[0];
                var pgPass = pgUserPass.Split(":")[1];
                var pgHost = pgHostPort.Split(":")[0];
                var pgPort = pgHostPort.Split(":")[1];

                connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb}; SSL Mode=Require; Trust Server Certificate=true";
            }

            // Whether the connection string came from the local development configuration file
            // or from the environment variable from Heroku, use it to set up your DbContext.


            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                connStr,
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }

        services.AddScoped<IPhotoAccessor, PhotoAccessor>();
        services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));

        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<IIdentityService, IdentityService>();

        return services;
    }
}
