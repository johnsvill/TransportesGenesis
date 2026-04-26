using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using TransportesGenesis.Data.Context;

public static class Startup
{
    public static WebApplication InicializarApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder);

        var app = builder.Build();
        Configure(app);
        return app;
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("TransportesGenesisConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();

        // AutoMapper (Geolocalización - NO AFECTA MÓDULOS EXISTENTES)
        builder.Services.AddAutoMapper(typeof(Startup).Assembly);

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

        builder.Services.AddControllersWithViews();
    }

    private static void Configure(WebApplication app)
    {
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

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();
    }
}
