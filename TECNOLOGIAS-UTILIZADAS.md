# 🛠️ TECNOLOGÍAS UTILIZADAS - TRANSPORTES GÉNESIS

## 📋 STACK TECNOLÓGICO COMPLETO

### 🎯 **ARQUITECTURA PRINCIPAL**
**Patrón:** Model-View-Controller (MVC) + Razor Pages  
**Framework:** ASP.NET Core 8.0  
**Lenguaje:** C# 12.0

---

## 🖥️ BACKEND TECHNOLOGIES

### **1. Framework y Runtime**
```yaml
- .NET 8.0 (LTS)
- ASP.NET Core 8.0
- Entity Framework Core 8.0
- ASP.NET Core Identity
```

### **2. Lenguaje de Programación**
```csharp
// C# 12.0 con características modernas
- Record types
- Pattern matching
- Nullable reference types
- Top-level statements
- Global using directives
```

### **3. Arquitectura de Datos**
```yaml
Database:
  - SQL Server (Entity Framework Core)
  - Code-First Migrations
  - LINQ for data queries
  - Repository Pattern (parcial)

Models:
  - Data Annotations
  - Fluent API Configuration
  - Value Converters
  - Audit Fields (CreatedAt, UpdatedAt)
```

### **4. Seguridad y Autenticación**
```csharp
// ASP.NET Core Identity configurado
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

// Autorización basada en roles
[Authorize(Roles = "Administrador")]
[Authorize(Roles = "PadreDeFamilia")]
```

### **5. Dependency Injection**
```csharp
// Contenedor de servicios nativo de .NET
services.AddScoped<IUserManager, UserManager>();
services.AddScoped<IBusService, BusService>();
services.AddScoped<IRutaService, RutaService>();
services.AddScoped<IAsistenciaService, AsistenciaService>();
```

---

## 🌐 FRONTEND TECHNOLOGIES

### **1. View Engine**
```razor
<!-- Razor Pages (.NET 8) -->
@page "/Padres/ConfirmarAsistencia"
@model TransportesGenesis.Pages.Padres.ConfirmarAsistenciaModel
@{
    ViewData["Title"] = "Confirmar Asistencia";
}

<!-- Tag Helpers -->
<form asp-page-handler="CrearUsuario" method="post">
    <input asp-for="Input.UserName" class="form-control" />
</form>
```

### **2. CSS Framework**
```css
/* Bootstrap 5.3.0 */
@import "bootstrap/dist/css/bootstrap.min.css";

/* Font Awesome 6.5.1 */
@import "fontawesome/css/all.min.css";

/* CSS Personalizado */
.attendance-header {
    background: linear-gradient(135deg, #FFA500 0%, #FF8C00 100%);
    color: white;
    border-radius: 15px;
}
```

### **3. JavaScript Moderno**
```javascript
// ES6+ Features utilizadas
- Arrow functions
- Template literals
- Async/await
- Destructuring
- Modules (import/export)
- Promises
- Fetch API

// Ejemplo de implementación
const actualizarMapa = async () => {
    try {
        const response = await fetch('/api/ubicaciones');
        const buses = await response.json();
        buses.forEach(bus => actualizarPosicionBus(bus));
    } catch (error) {
        console.error('Error actualizando mapa:', error);
    }
};
```

### **4. Librerías de Mapas**
```javascript
// Leaflet.js 1.9.4
L.map('mapa', {
    center: [14.6349, -90.5069], // Guatemala City
    zoom: 13
});

// Plugins utilizados
- Leaflet Routing Machine
- Leaflet MarkerCluster
- Custom Bus Icons
- Real-time tracking
```

---

## 🔌 LIBRERÍAS Y DEPENDENCIAS

### **1. Backend NuGet Packages**
```xml
<PackageReference Include="Microsoft.AspNetCore.App" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="AutoMapper" Version="12.0.1" />
<PackageReference Include="Stripe.net" Version="43.0.0" />
<PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
```

### **2. Frontend Libraries (CDN)**
```html
<!-- CSS Frameworks -->
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" />
<link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/all.min.css" />

<!-- JavaScript Libraries -->
<script src="https://unpkg.com/leaflet@1.9.4/dist/leaflet.js"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

<!-- Validaciones -->
<script src="~/lib/jquery-validation/dist/jquery.validate.min.js"></script>
<script src="~/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js"></script>
```

---

## 🗺️ INTEGRACIÓN DE MAPAS

### **Leaflet.js - Configuración Completa**
```javascript
// Inicialización del mapa
const mapa = L.map('mapContainer', {
    center: [14.6349, -90.5069], // Guatemala City coordinates
    zoom: 13,
    zoomControl: true,
    scrollWheelZoom: true
});

// Capas de mapas
L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '© OpenStreetMap contributors',
    maxZoom: 18
}).addTo(mapa);

// Iconos personalizados para buses
const busIcon = L.icon({
    iconUrl: '/images/bus-icon.png',
    iconSize: [32, 32],
    iconAnchor: [16, 16],
    popupAnchor: [0, -16]
});

// Rutas con Leaflet Routing Machine
L.Routing.control({
    waypoints: rutaWaypoints,
    routeWhileDragging: false,
    createMarker: function() { return null; }
}).addTo(mapa);
```

---

## 💾 BASE DE DATOS

### **1. SQL Server con Entity Framework Core**
```csharp
// Configuración de DbContext
public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public DbSet<Bus> Buses { get; set; }
    public DbSet<Ruta> Rutas { get; set; }
    public DbSet<Parada> Paradas { get; set; }
    public DbSet<AsistenciaAlumno> AsistenciasAlumnos { get; set; }
    public DbSet<PagoPadre> PagosPadres { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
```

### **2. Migraciones Code-First**
```powershell
# Comandos utilizados
Add-Migration InitialCreate
Add-Migration AddPagosSystem
Add-Migration AddGeolocalizacionSystem
Update-Database
```

### **3. Configuraciones de Entidades**
```csharp
// Fluent API Configuration
public class BusConfig : IEntityTypeConfiguration<Bus>
{
    public void Configure(EntityTypeBuilder<Bus> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Placa).IsRequired().HasMaxLength(10);
        builder.Property(b => b.Modelo).HasMaxLength(50);
        builder.HasOne(b => b.Ruta).WithMany(r => r.Buses);
    }
}
```

---

## 🔄 TIEMPO REAL Y SIGNALR

### **SignalR Hub**
```csharp
// Hub para notificaciones en tiempo real
public class NotificacionesHub : Hub
{
    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task SendNotification(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}

// Configuración en Startup
services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
});
```

---

## 💳 INTEGRACIÓN DE PAGOS

### **Stripe Payment Processing**
```csharp
// Configuración de Stripe
public class StripeSettings
{
    public string SecretKey { get; set; }
    public string PublishableKey { get; set; }
}

// Implementación de pagos
var options = new PaymentIntentCreateOptions
{
    Amount = (long)(request.Monto * 100), // Centavos
    Currency = "gtq", // Quetzales guatemaltecos
    Metadata = new Dictionary<string, string>
    {
        { "UsuarioId", User.Identity.Name },
        { "Mes", request.Mes },
        { "Anio", DateTime.Now.Year.ToString() }
    }
};
```

---

## 🏗️ PATRONES DE DISEÑO IMPLEMENTADOS

### **1. Repository Pattern (Parcial)**
```csharp
public interface IRepositoryBase<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}
```

### **2. Dependency Injection**
```csharp
// Registro de servicios
services.AddScoped<IUbicacionBusService, UbicacionBusService>();
services.AddScoped<IRutaRepository, RutaRepository>();
services.AddScoped<IAsistenciaAlumnoRepository, AsistenciaAlumnoRepository>();
```

### **3. AutoMapper**
```csharp
// Mapeo de DTOs
public class RutaMappingProfile : Profile
{
    public RutaMappingProfile()
    {
        CreateMap<Ruta, RutaDto>();
        CreateMap<Bus, BusDto>();
        CreateMap<UbicacionBusEnTiempoReal, UbicacionBusDto>();
    }
}
```

---

## 📱 RESPONSIVE DESIGN

### **CSS Media Queries**
```css
/* Mobile First Approach */
.dashboard-card {
    padding: 1rem;
    margin-bottom: 1rem;
}

/* Tablet */
@media (min-width: 768px) {
    .dashboard-card {
        padding: 1.5rem;
    }
}

/* Desktop */
@media (min-width: 1200px) {
    .dashboard-card {
        padding: 2rem;
    }
}
```

### **Bootstrap Grid System**
```html
<!-- Layout responsivo -->
<div class="container-fluid">
    <div class="row">
        <div class="col-12 col-md-6 col-lg-4">
            <!-- Contenido adaptativo -->
        </div>
    </div>
</div>
```

---

## 🔧 HERRAMIENTAS DE DESARROLLO

### **IDE y Entorno**
```yaml
IDE: Microsoft Visual Studio Community 2026 (18.5.1)
Runtime: .NET 8.0 SDK
Package Manager: NuGet
Version Control: Git + GitHub
Database: SQL Server Express LocalDB
Browser Testing: Chrome, Edge, Firefox
```

### **Scripts de Build**
```json
{
  "scripts": {
    "build": "dotnet build",
    "test": "dotnet test",
    "run": "dotnet run",
    "watch": "dotnet watch run"
  }
}
```

---

## 🚀 DEPLOYMENT Y HOSTING

### **Configuración de Producción**
```csharp
// appsettings.Production.json
{
  "ConnectionStrings": {
    "TransportesGenesisConnection": "Server=prod;Database=TransportesGenesis;..."
  },
  "Stripe": {
    "SecretKey": "sk_live_...",
    "PublishableKey": "pk_live_..."
  }
}
```

### **Docker Support (Preparado)**
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TransportesGenesis.csproj", "."]
RUN dotnet restore "TransportesGenesis.csproj"
```

---

## 📊 MÉTRICAS Y PERFORMANCE

### **Optimizaciones Implementadas**
```csharp
// Lazy Loading configurado
services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseLazyLoadingProxies();
    options.EnableSensitiveDataLogging(false);
});

// Caching en memoria
services.AddMemoryCache();
services.AddResponseCaching();
```

### **Bundle y Minificación**
```html
<!-- Optimización de recursos -->
<environment include="Development">
    <link rel="stylesheet" href="~/css/site.css" />
</environment>
<environment exclude="Development">
    <link rel="stylesheet" href="~/css/site.min.css" asp-append-version="true" />
</environment>
```

---

## 🛡️ SEGURIDAD IMPLEMENTADA

### **1. Autenticación y Autorización**
```csharp
// Configuración de cookies seguras
services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Auth/Logout";
    options.AccessDeniedPath = "/Auth/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(24);
    options.SlidingExpiration = true;
});
```

### **2. Validación y Sanitización**
```csharp
// Data Annotations
public class UsuarioInputModel
{
    [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
    [StringLength(50, ErrorMessage = "Máximo 50 caracteres")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "Formato de correo inválido")]
    public string Email { get; set; }
}
```

### **3. CSRF Protection**
```html
<!-- Anti-forgery tokens -->
<form asp-antiforgery="true" method="post">
    @Html.AntiForgeryToken()
</form>
```

---

## 📈 ESCALABILIDAD Y MANTENIMIENTO

### **Arquitectura Modular**
```
📁 TransportesGenesis/
├── 📁 Controllers/     # Lógica de controladores
├── 📁 Models/          # Entidades y ViewModels  
├── 📁 Pages/           # Razor Pages
├── 📁 Services/        # Lógica de negocio
├── 📁 Repositories/    # Acceso a datos
├── 📁 Data/            # Configuraciones EF Core
├── 📁 DTOs/            # Transfer Objects
├── 📁 Mappings/        # AutoMapper profiles
└── 📁 wwwroot/         # Assets estáticos
```

### **Separación de Responsabilidades**
- **Controllers:** Manejo de HTTP requests
- **Services:** Lógica de negocio
- **Repositories:** Acceso a datos
- **DTOs:** Transferencia de datos
- **Models:** Entidades del dominio

---

**📅 Última Actualización:** Mayo 2026  
**🏗️ Arquitectura:** .NET 8 + Razor Pages + JavaScript + SQL Server  
**⚡ Performance:** Optimizado para producción  
**🔒 Seguridad:** Implementada según mejores prácticas .NET  
**📱 Compatibilidad:** Cross-platform (Windows, Linux, macOS)