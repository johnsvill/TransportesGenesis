using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TransportesGenesis.Data.Context;
using TransportesGenesis.Models.Config;
using TransportesGenesis.Models.DB.Usuarios;
using TransportesGenesis.Services;

public static class Startup
{
    public static WebApplication InicializarApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();
        Configure(app, app.Environment);
        return app;
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // Connection string
        var connectionString = configuration.GetConnectionString("TransportesGenesisConnection")
            ?? throw new InvalidOperationException("Connection string 'TransportesGenesisConnection' not found.");

        // Database context
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddDatabaseDeveloperPageExceptionFilter();

        // Identity configuration (de tu compañero)
        services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // Configurar rutas de autenticación personalizadas (de tu compañero)
        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Auth/Login";
            options.LogoutPath = "/Auth/Logout";
            options.AccessDeniedPath = "/Auth/AccessDenied";
        });

        // Email sender (de tu compañero)
        services.AddTransient<IEmailSender, EmailSender>();

        // Stripe configuration (de tu compañero)
        services.Configure<StripeSettings>(configuration.GetSection("Stripe"));

        // AutoMapper (Geolocalización)
        services.AddAutoMapper(typeof(Startup).Assembly);

        // HttpClient para llamadas internas a APIs
        services.AddHttpClient();

        // Repositorios de Geolocalización
        services.AddScoped<TransportesGenesis.Repositories.Interfaces.IBusRepository, TransportesGenesis.Repositories.Implementations.BusRepository>();
        services.AddScoped<TransportesGenesis.Repositories.Interfaces.IRutaRepository, TransportesGenesis.Repositories.Implementations.RutaRepository>();
        services.AddScoped<TransportesGenesis.Repositories.Interfaces.IUbicacionBusRepository, TransportesGenesis.Repositories.Implementations.UbicacionBusRepository>();
        services.AddScoped<TransportesGenesis.Repositories.Interfaces.IAsistenciaAlumnoRepository, TransportesGenesis.Repositories.Implementations.AsistenciaAlumnoRepository>();
        services.AddScoped<TransportesGenesis.Repositories.Interfaces.ISolicitudTrasladoRepository, TransportesGenesis.Repositories.Implementations.SolicitudTrasladoRepository>();

        // Services de Geolocalización
        services.AddScoped<TransportesGenesis.Services.Interfaces.IBusService, TransportesGenesis.Services.Implementations.BusService>();
        services.AddScoped<TransportesGenesis.Services.Interfaces.IUbicacionBusService, TransportesGenesis.Services.Implementations.UbicacionBusService>();
        services.AddScoped<TransportesGenesis.Services.Interfaces.IAsistenciaService, TransportesGenesis.Services.Implementations.AsistenciaService>();
        services.AddScoped<TransportesGenesis.Services.Interfaces.ITrasladoService, TransportesGenesis.Services.Implementations.TrasladoService>();
        services.AddScoped<TransportesGenesis.Services.Interfaces.IRutaService, TransportesGenesis.Services.Implementations.RutaService>();
        services.AddScoped<TransportesGenesis.Services.Interfaces.IConfiguracionService, TransportesGenesis.Services.Implementations.ConfiguracionService>();
        services.AddScoped<TransportesGenesis.Services.Interfaces.INotificacionService, TransportesGenesis.Services.Implementations.NotificacionService>();

        // SignalR para notificaciones en tiempo real
        services.AddSignalR(options =>
        {
            options.EnableDetailedErrors = true;
            options.KeepAliveInterval = TimeSpan.FromSeconds(15);
            options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
        });

        // MVC y Razor Pages
        services.AddControllersWithViews();
        services.AddRazorPages().AddRazorRuntimeCompilation();
    }

    private static void Configure(WebApplication app, IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline
        if (env.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        // Configuración de Stripe (de tu compañero)
        var stripeSettings = app.Services.GetRequiredService<IOptions<StripeSettings>>().Value;
        Stripe.StripeConfiguration.ApiKey = stripeSettings.SecretKey;

        // Seed de roles y usuario admin (de tu compañero)
        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            SeedRolesAndAdmin(roleManager, userManager).GetAwaiter().GetResult();
        }

        // SignalR endpoint para notificaciones
        app.MapHub<TransportesGenesis.Hubs.NotificacionesHub>("/notificacionesHub");

        // Mapear rutas de controladores MVC ANTES que Razor Pages para que AuthController tenga prioridad
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        // Habilitar Razor Pages (necesarias para Admin, Padres, Geolocalizacion, etc.)
        app.MapRazorPages();
    }

    private static async Task SeedRolesAndAdmin(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
    {
        string[] roleNames = { "Administrador", "Piloto", "Monitor", "PadreDeFamilia" };

        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        var adminEmail = "admin@transportesgenesis.com";
        var adminPassword = "Admin123!";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            var newAdmin = new AppUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                IsFirstLogin = false
            };

            var result = await userManager.CreateAsync(newAdmin, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newAdmin, "Administrador");
            }
        }
    }
}
