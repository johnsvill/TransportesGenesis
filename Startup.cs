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
            ?? throw new InvalidOperationException("Connection string 'TransportesGenesisConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddDatabaseDeveloperPageExceptionFilter();

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

        services.AddTransient<IEmailSender, EmailSender>();
        services.AddControllersWithViews();
        services.AddRazorPages().AddRazorRuntimeCompilation();
        services.Configure<StripeSettings>(configuration.GetSection("Stripe"));
    }

    private static void Configure(WebApplication app, IWebHostEnvironment env)
    {
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
