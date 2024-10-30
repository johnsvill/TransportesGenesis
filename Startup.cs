using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
<<<<<<< HEAD
using TransportesGenesis.Data;
=======
using TransportesGenesis.Data.Context;
>>>>>>> dev_jonathan

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
<<<<<<< HEAD
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
=======
        var connectionString = builder.Configuration.GetConnectionString("TransportesGenesisConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

>>>>>>> dev_jonathan
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();
<<<<<<< HEAD
=======

>>>>>>> dev_jonathan
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
