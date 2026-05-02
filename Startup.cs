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
        var connectionString = configuration.GetConnectionString("TransportesGenesisConnection")
        var connectionString = builder.Configuration.GetConnectionString("TransportesGenesisConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            ?? throw new InvalidOperationException("Connection string 'TransportesGenesisConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();
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

        // AutoMapper (Geolocalización - NO AFECTA MÓDULOS EXISTENTES)
        builder.Services.AddAutoMapper(typeof(Startup).Assembly);

        // HttpClient para llamadas internas a APIs
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddControllersWithViews();
        services.AddRazorPages().AddRazorRuntimeCompilation();
        services.Configure<StripeSettings>(configuration.GetSection("Stripe"));
        builder.Services.AddHttpClient();

        // Repositorios de Geolocalización (NUEVOS - NO AFECTAN MÓDULO DE PAGOS)
        builder.Services.AddScoped<TransportesGenesis.Repositories.Interfaces.IBusRepository, TransportesGenesis.Repositories.Implementations.BusRepository>();
        builder.Services.AddScoped<TransportesGenesis.Repositories.Interfaces.IRutaRepository, TransportesGenesis.Repositories.Implementations.RutaRepository>();
        builder.Services.AddScoped<TransportesGenesis.Repositories.Interfaces.IUbicacionBusRepository, TransportesGenesis.Repositories.Implementations.UbicacionBusRepository>();
        builder.Services.AddScoped<TransportesGenesis.Repositories.Interfaces.IAsistenciaAlumnoRepository, TransportesGenesis.Repositories.Implementations.AsistenciaAlumnoRepository>();
        builder.Services.AddScoped<TransportesGenesis.Repositories.Interfaces.ISolicitudTrasladoRepository, TransportesGenesis.Repositories.Implementations.SolicitudTrasladoRepository>();

        // Services de Geolocalización (NUEVOS - NO AFECTAN MÓDULO DE PAGOS)
        builder.Services.AddScoped<TransportesGenesis.Services.Interfaces.IBusService, TransportesGenesis.Services.Implementations.BusService>();
        builder.Services.AddScoped<TransportesGenesis.Services.Interfaces.IUbicacionBusService, TransportesGenesis.Services.Implementations.UbicacionBusService>();
        builder.Services.AddScoped<TransportesGenesis.Services.Interfaces.IAsistenciaService, TransportesGenesis.Services.Implementations.AsistenciaService>();
        builder.Services.AddScoped<TransportesGenesis.Services.Interfaces.ITrasladoService, TransportesGenesis.Services.Implementations.TrasladoService>();
        builder.Services.AddScoped<TransportesGenesis.Services.Interfaces.IRutaService, TransportesGenesis.Services.Implementations.RutaService>(); // FASE 5: Cálculo dinámico de rutas
        builder.Services.AddScoped<TransportesGenesis.Services.Interfaces.IConfiguracionService, TransportesGenesis.Services.Implementations.ConfiguracionService>(); // FASE 5: Configuración del sistema
        builder.Services.AddScoped<TransportesGenesis.Services.Interfaces.INotificacionService, TransportesGenesis.Services.Implementations.NotificacionService>(); // FASE 7: Notificaciones SignalR

        // FASE 7: SignalR para notificaciones en tiempo real
        builder.Services.AddSignalR(options =>
        {
            options.EnableDetailedErrors = true; // Solo en desarrollo
            options.KeepAliveInterval = TimeSpan.FromSeconds(15);
            options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
        });

        builder.Services.AddControllersWithViews();
    }

    private static void Configure(WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
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

        // FASE 7: Endpoint de SignalR para notificaciones
        app.MapHub<TransportesGenesis.Hubs.NotificacionesHub>("/notificacionesHub");
        var stripeSettings = app.Services.GetRequiredService<IOptions<StripeSettings>>().Value;
        Stripe.StripeConfiguration.ApiKey = stripeSettings.SecretKey;

        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            SeedRolesAndAdmin(roleManager, userManager).GetAwaiter().GetResult();
        }

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
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
