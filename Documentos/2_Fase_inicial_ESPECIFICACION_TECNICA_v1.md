# 🔧 ESPECIFICACIÓN TÉCNICA - TRANSPORTES GENESIS
## Sistema de Gestión de Transporte Escolar

---

**Proyecto:** Transportes Genesis  
**Tipo de Documento:** Especificación Técnica  
**Versión:** 1.0  
**Fecha:** 2025  
**Autor:** Equipo de Desarrollo TransportesGenesis  
**Repositorio:** https://github.com/johnsvill/TransportesGenesis  
**Rama:** dev_david  
**Tecnología Principal:** .NET 8 con Razor Pages  

---

## 📑 ÍNDICE

1. [Arquitectura de referencia](#21-arquitectura-de-referencia)
2. [Stack tecnológico seleccionado](#22-stack-tecnológico-seleccionado)
3. [Requerimientos de infraestructura y entornos](#23-requerimientos-de-infraestructura-y-entornos)
4. [Diseño de base de datos](#24-diseño-de-base-de-datos)
5. [Diseño de APIs e integración](#25-diseño-de-apis-e-integración)
6. [Patrones de diseño y buenas prácticas](#26-patrones-de-diseño-y-buenas-prácticas)
7. [Plan de implementación y roadmap técnico](#27-plan-de-implementación-y-roadmap-técnico)
8. [Análisis de riesgos técnicos](#28-análisis-de-riesgos-técnicos)

---

## 2.1 Arquitectura de referencia

### 2.1.1 Diagrama C4 - Nivel 1: Contexto del Sistema

**[INSTRUCCIONES PARA DIAGRAMA]**

```
Título: Diagrama de Contexto - Transportes Genesis

Elementos:
1. Sistema Central: "Transportes Genesis"
   - Tipo: Sistema de Software
   - Tecnología: ASP.NET Core 8 (Razor Pages)

2. Actores Externos (Personas):
   - Administrador (Gerente/Dueño)
   - Piloto (Conductor)
   - Monitor (Asistente del bus)
   - Padre de Familia (Cliente)

3. Sistemas Externos:
   - Google Maps API (Geolocalización y mapas)
   - Navegador Web (Chrome, Firefox, Edge, Safari)
   - [Futuro] Pasarela de Pagos (Stripe, PayPal)

Relaciones:
- Administrador → Transportes Genesis: "Gestiona usuarios, buses, rutas"
- Piloto → Transportes Genesis: "Consulta su ruta asignada"
- Monitor → Transportes Genesis: "Registra recogidas de alumnos"
- Padre → Transportes Genesis: "Confirma asistencia, consulta ubicación"
- Transportes Genesis → Google Maps API: "Obtiene mapas y geocodificación"
- Usuarios → Navegador Web → Transportes Genesis: "Accede vía HTTPS"

Notas:
- Todos los usuarios acceden vía navegador web (responsive)
- No hay aplicación móvil nativa en v1.0
- Comunicación HTTPS segura en todas las conexiones
```

---

### 2.1.2 Diagrama C4 - Nivel 2: Contenedores

**[INSTRUCCIONES PARA DIAGRAMA]**

```
Título: Diagrama de Contenedores - Transportes Genesis

Contenedores:

1. "Web Application" (ASP.NET Core Razor Pages)
   - Tecnología: .NET 8, Razor Pages, MVC Controllers
   - Puerto: 443 (HTTPS)
   - Responsabilidad: Interfaz de usuario, lógica de presentación
   - Componentes: Pages/, Controllers/, Views/

2. "API REST" (ASP.NET Core Web API)
   - Tecnología: .NET 8, ASP.NET Core Controllers
   - Responsabilidad: Endpoints para operaciones CRUD, cálculo de rutas
   - Ruta base: /api/*

3. "SignalR Hub" (Tiempo Real)
   - Tecnología: SignalR for .NET 8
   - Responsabilidad: Comunicación bidireccional en tiempo real
   - Uso: Geolocalización de buses en vivo

4. "Base de Datos" (SQL Server)
   - Tecnología: SQL Server 2019+
   - Puerto: 1433
   - Responsabilidad: Almacenamiento persistente de datos
   - Esquemas: genesis (negocio), dbo (Identity)

5. "Cliente Web" (Navegador)
   - Tecnología: HTML5, CSS3, JavaScript (jQuery)
   - Frameworks: Bootstrap 5, SignalR Client JS
   - Responsabilidad: Renderizado de UI, interacción con usuario

Relaciones:
- Cliente Web → Web Application: "HTTPS requests" (puerto 443)
- Cliente Web → SignalR Hub: "WebSocket connection"
- Web Application → Base de Datos: "SQL queries via EF Core"
- API REST → Base de Datos: "SQL queries via EF Core"
- SignalR Hub → Base de Datos: "SQL queries (opcional)"
- Web Application → Google Maps API: "HTTPS API calls"

Notas:
- Todos los contenedores se despliegan en un mismo servidor/App Service
- SignalR usa WebSockets (fallback a Server-Sent Events o Long Polling)
- Entity Framework Core como ORM entre aplicación y BD
```

---

### 2.1.3 Diagrama C4 - Nivel 3: Componentes (Web Application)

**[INSTRUCCIONES PARA DIAGRAMA]**

```
Título: Diagrama de Componentes - Web Application

Componentes principales:

1. "Authentication & Authorization" (ASP.NET Core Identity)
   - Archivos: Controllers/AuthController.cs, Startup.cs
   - Responsabilidad: Login, logout, gestión de sesiones
   - Usa: AspNetUsers, AspNetRoles, AspNetUserRoles

2. "Pages - Admin" (Razor Pages)
   - Carpeta: Pages/Admin/
   - Páginas: Index.cshtml, CalcularRutas.cshtml, CrearUsuariosPrueba.cshtml
   - Responsabilidad: Panel de administración

3. "Pages - Piloto" (Razor Pages)
   - Carpeta: Pages/Piloto/
   - Páginas: MiRuta.cshtml
   - Responsabilidad: Vista de ruta para pilotos

4. "Pages - Monitor" (Razor Pages)
   - Carpeta: Pages/Monitor/
   - Páginas: MiRuta.cshtml, RegistrarRecogidas.cshtml
   - Responsabilidad: Vista de ruta y registro de recogidas

5. "Pages - Padres" (Razor Pages)
   - Carpeta: Pages/Padres/
   - Páginas: ConfirmarAsistencia.cshtml
   - Responsabilidad: Confirmación de asistencia de alumnos

6. "Pages - Geolocalización" (Razor Pages)
   - Carpeta: Pages/Geolocalizacion/
   - Páginas: MapaEnTiempoReal.cshtml
   - Responsabilidad: Mapa en vivo con ubicación de buses

7. "API Controllers" (MVC Controllers)
   - Archivos: Controllers/RutasController.cs, Controllers/PagosController.cs
   - Responsabilidad: Endpoints REST para operaciones

8. "SignalR Hub"
   - Archivos: Hubs/GeolocalizacionHub.cs
   - Responsabilidad: Broadcast de ubicaciones de buses

9. "Data Access Layer" (Entity Framework Core)
   - Archivos: Data/Context/ApplicationDbContext.cs
   - Responsabilidad: Acceso a base de datos, DbSets

10. "Models - Domain"
    - Carpeta: Models/DB/Negocio/, Models/DB/Usuarios/
    - Archivos: Bus.cs, Ruta.cs, Parada.cs, Alumnos.cs, etc.
    - Responsabilidad: Entidades de dominio

11. "DTOs"
    - Carpeta: DTOs/
    - Archivos: RutaDto.cs, PagoDto.cs, etc.
    - Responsabilidad: Transferencia de datos entre capas

Relaciones:
- Authentication → Data Access Layer: "Valida credenciales"
- Pages → Data Access Layer: "Lee/escribe datos"
- API Controllers → Data Access Layer: "CRUD operations"
- SignalR Hub → Clientes: "Broadcast messages"
- Data Access Layer → SQL Server: "SQL queries"

Notas:
- Patrón Page Model (Razor Pages) para UI
- Dependency Injection para todos los componentes
- Separation of Concerns (presentación, lógica, datos)
```

---

### 2.1.4 Estilo Arquitectónico y Justificación

#### Estilo Arquitectónico Principal

**Razor Pages con Patrón Page-Focused MVC**

El proyecto está construido principalmente con **Razor Pages**, un patrón de diseño de ASP.NET Core que simplifica el desarrollo de aplicaciones web orientadas a páginas. Este estilo se complementa con:

- **MVC Controllers** para endpoints API REST
- **SignalR Hub** para comunicación en tiempo real
- **Repository Pattern** implícito mediante Entity Framework Core

#### Capas de la Aplicación

La arquitectura sigue una **separación de responsabilidades en 3 capas lógicas**:

```
┌─────────────────────────────────────────────────┐
│         CAPA DE PRESENTACIÓN                    │
│  - Razor Pages (.cshtml + .cshtml.cs)          │
│  - MVC Controllers (para API)                   │
│  - ViewModels / DTOs                            │
│  - SignalR Hubs                                 │
└─────────────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────────────┐
│         CAPA DE LÓGICA DE NEGOCIO               │
│  - PageModels (OnGet, OnPost methods)          │
│  - Business Logic en Controllers                │
│  - Validaciones de dominio                      │
│  - Reglas de negocio (ej: cálculo de rutas)    │
└─────────────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────────────┐
│         CAPA DE ACCESO A DATOS                  │
│  - ApplicationDbContext (EF Core)               │
│  - Entidades de dominio (Models/DB/)            │
│  - Migrations                                   │
│  - DbSets<T> como Repositories                  │
└─────────────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────────────┐
│         BASE DE DATOS                           │
│  - SQL Server 2019+                             │
│  - Esquema 'genesis' (negocio)                  │
│  - Esquema 'dbo' (ASP.NET Core Identity)        │
└─────────────────────────────────────────────────┘
```

#### Justificación de la Arquitectura Elegida

**¿Por qué Razor Pages?**

1. **Productividad**: Desarrollo rápido de páginas web sin complejidad excesiva
2. **Simplicidad**: Patrón Page-Focused más intuitivo que MVC tradicional para aplicaciones CRUD
3. **Integración nativa con .NET**: Soporte oficial de Microsoft, excelente documentación
4. **Separación de concerns**: El PageModel separa lógica de presentación (CSHTML) de lógica de negocio (CS)
5. **Madurez**: Tecnología estable y probada en producción

**¿Por qué complementar con MVC Controllers?**

- Para **endpoints API REST** (`/api/*`) que no requieren renderizado de UI
- Separación clara entre páginas web (Razor Pages) y servicios API (Controllers)

**¿Por qué SignalR?**

- **Comunicación en tiempo real** necesaria para geolocalización de buses
- Protocolo **WebSocket** eficiente (bajo overhead)
- Integración nativa con ASP.NET Core

**Alternativas consideradas:**

| Alternativa | Ventaja | Desventaja | Decisión |
|-------------|---------|------------|----------|
| **Blazor Server** | UI más reactiva, C# en cliente | Mayor complejidad, estado en servidor | ❌ Rechazado (overkill para v1.0) |
| **Blazor WebAssembly** | SPA, C# en navegador | Tamaño de descarga grande, curva de aprendizaje | ❌ Rechazado (no necesario) |
| **Angular/React + API** | SPA moderna, experiencia de usuario rica | Requiere equipo frontend separado, más tiempo de desarrollo | ❌ Rechazado (fuera de alcance) |
| **MVC tradicional** | Patrón conocido | Más verboso que Razor Pages | ⚠️ Usado solo para API |
| **Razor Pages + MVC + SignalR** | Balance perfecto, productividad alta, tiempo real | Ninguna significativa | ✅ **ELEGIDO** |

---

### 2.1.5 Diagrama de Despliegue

**[INSTRUCCIONES PARA DIAGRAMA UML]**

```
Título: Diagrama de Despliegue - Transportes Genesis

Nodos:

1. "Cliente" (Device)
   - Tipo: Dispositivo de usuario (PC, Tablet, Smartphone)
   - Software: Navegador Web (Chrome, Firefox, Edge, Safari)
   - Componentes: 
     - HTML5/CSS3/JavaScript
     - Bootstrap 5 (UI)
     - SignalR Client (JS)

2. "Servidor Web / App Service" (Execution Environment)
   - Tipo: IIS 10 / Azure App Service
   - Sistema Operativo: Windows Server 2019+ / Azure
   - Runtime: .NET 8 Runtime
   - Componentes:
     - ASP.NET Core Application (Razor Pages + API)
     - SignalR Hub
     - Kestrel Web Server (detrás de IIS)

3. "Servidor de Base de Datos" (Database Server)
   - Tipo: SQL Server 2019+
   - Sistema Operativo: Windows Server / Azure SQL Database
   - Puerto: 1433 (TCP)
   - Componentes:
     - SQL Server Database Engine
     - Base de datos: TransportesGenesisDb
     - Esquemas: genesis, dbo

4. "Servicios Externos" (External System)
   - Google Maps API (HTTPS, puerto 443)
   - [Futuro] Pasarela de Pagos

Conexiones:

- Cliente → Servidor Web: "HTTPS (puerto 443)"
  - Protocolo: HTTP/1.1, HTTP/2
  - Seguridad: TLS 1.2+

- Cliente → SignalR Hub: "WebSocket (puerto 443 upgrade)"
  - Protocolo: WebSocket (fallback: SSE, Long Polling)
  - Frecuencia: cada 5-10 segundos

- Servidor Web → SQL Server: "SQL Protocol (puerto 1433)"
  - Protocolo: TDS (Tabular Data Stream)
  - Conexión: Connection Pooling habilitado
  - String de conexión cifrada

- Servidor Web → Google Maps API: "HTTPS (puerto 443)"
  - Protocolo: REST over HTTPS
  - Autenticación: API Key

Configuración de Red:
- Firewall: Solo puertos 443 (HTTPS) y 1433 (SQL) abiertos
- Load Balancer (opcional): Para alta disponibilidad
- CDN (opcional): Para contenido estático (CSS, JS, imágenes)

Notas:
- En desarrollo: Servidor Web y SQL Server pueden estar en la misma máquina
- En producción: Separar en servidores distintos o usar Azure App Service + Azure SQL
- Certificado SSL requerido (Let's Encrypt gratuito o comercial)
```

---

### 2.1.6 Principios de Diseño Aplicados

El diseño arquitectónico del sistema se basa en los siguientes principios:

#### 1. **Separation of Concerns (SoC)**
- La lógica de presentación (CSHTML) está separada de la lógica de negocio (PageModel)
- La capa de datos (ApplicationDbContext) está aislada del resto de la aplicación
- Cada componente tiene una responsabilidad única y bien definida

#### 2. **Dependency Injection (DI)**
- Todos los servicios se registran en `Startup.cs` (o `Program.cs` en .NET 8)
- Las dependencias se inyectan vía constructor
- Facilita testing y desacoplamiento

**Ejemplo:**
```csharp
public MiRutaModel(
    IHttpClientFactory httpClientFactory,
    ApplicationDbContext context)
{
    _httpClientFactory = httpClientFactory;
    _context = context;
}
```

#### 3. **Don't Repeat Yourself (DRY)**
- Uso de componentes parciales (`_Layout.cshtml`)
- DTOs reutilizables (`RutaDto`, `ParadaRutaDto`)
- Clases base con auditoría (`Auditoria.cs`)

#### 4. **SOLID Principles**
- **S**ingle Responsibility: Cada PageModel tiene una única responsabilidad
- **O**pen/Closed: Extensible mediante herencia (ej: `Auditoria` base)
- **L**iskov Substitution: Clases derivadas son intercambiables
- **I**nterface Segregation: Interfaces específicas (ej: `IHttpClientFactory`)
- **D**ependency Inversion: Dependencias a abstracciones, no implementaciones

#### 5. **Convention over Configuration**
- Razor Pages usa convenciones de carpetas (`Pages/`)
- Entity Framework usa convenciones de nomenclatura
- Reducción de configuración manual

#### 6. **Security by Design**
- Autenticación obligatoria (`[Authorize]`)
- Autorización basada en roles (`[Authorize(Roles = "...")]`)
- Anti-CSRF tokens en formularios
- HTTPS obligatorio
- Hashing de contraseñas con ASP.NET Core Identity

---

✅ **2.1 ARQUITECTURA DE REFERENCIA - COMPLETADO**

---

## 2.2 Stack tecnológico seleccionado

### 2.2.1 Tecnologías por Capa

#### Backend (Servidor)

| Tecnología | Versión | Licencia | Propósito | Justificación |
|------------|---------|----------|-----------|---------------|
| **.NET** | 8.0 LTS | MIT | Plataforma de desarrollo | Última versión LTS, soporte hasta 2026, alto rendimiento, multiplataforma |
| **ASP.NET Core** | 8.0 | MIT | Framework web | Framework moderno, cross-platform, alto rendimiento, excelente para APIs REST y Razor Pages |
| **Razor Pages** | 8.0 | MIT | Framework UI | Patrón simplificado para páginas web, productividad alta, menos verboso que MVC tradicional |
| **Entity Framework Core** | 8.0 | MIT | ORM | Acceso a datos con LINQ, migraciones automáticas, soporte para SQL Server, reduce código boilerplate |
| **ASP.NET Core Identity** | 8.0 | MIT | Autenticación/Autorización | Sistema completo de gestión de usuarios, roles, claims, integración nativa con EF Core |
| **SignalR** | 8.0 | MIT | Comunicación en tiempo real | WebSockets para geolocalización en vivo, fallback automático, integración nativa con ASP.NET Core |
| **C#** | 12.0 | MIT | Lenguaje de programación | Lenguaje tipado, moderno, orientado a objetos, async/await nativo, LINQ |

#### Frontend (Cliente)

| Tecnología | Versión | Licencia | Propósito | Justificación |
|------------|---------|----------|-----------|---------------|
| **HTML5** | - | W3C | Estructura de páginas | Estándar web moderno, soporte para semántica, accesibilidad |
| **CSS3** | - | W3C | Estilos y diseño | Flexbox, Grid, animaciones, variables CSS |
| **Bootstrap** | 5.3 | MIT | Framework CSS | UI responsive, componentes pre-diseñados, grid system, iconos, compatible con todos los navegadores |
| **JavaScript** | ES6+ | - | Interactividad cliente | Lenguaje estándar de navegadores, soporte async/await, manipulación DOM |
| **jQuery** | 3.7.0 | MIT | Librería JS | Simplifica manipulación DOM, AJAX, eventos, amplia compatibilidad |
| **SignalR Client (JS)** | 8.0 | MIT | Cliente WebSocket | Cliente JavaScript para conexión con SignalR Hub, manejo de reconexiones |
| **Bootstrap Icons** | 1.11 | MIT | Iconografía | Iconos vectoriales, ligeros, consistentes con Bootstrap |

#### Base de Datos

| Tecnología | Versión | Licencia | Propósito | Justificación |
|------------|---------|----------|-----------|---------------|
| **SQL Server** | 2019+ | Comercial | RDBMS | Motor de base de datos empresarial, robustez, soporte de Microsoft, herramientas de administración, backups automáticos |
| **T-SQL** | - | - | Lenguaje de consultas | Extensión de SQL estándar, stored procedures, triggers, funciones |

#### Infraestructura y Despliegue

| Tecnología | Versión | Licencia | Propósito | Justificación |
|------------|---------|----------|-----------|---------------|
| **IIS** | 10.0+ | - | Servidor web | Servidor web de Microsoft, integración con Windows Server, soporte para .NET nativo |
| **Kestrel** | 8.0 | MIT | Servidor HTTP | Servidor web multiplataforma de .NET, alto rendimiento, usado internamente por ASP.NET Core |
| **Azure App Service** | - | Comercial (PaaS) | Hosting cloud (opcional) | PaaS de Microsoft, escalado automático, integración con Azure SQL, CI/CD integrado |
| **Windows Server** | 2019+ | Comercial | Sistema Operativo | SO estable, compatible con IIS y SQL Server, soporte empresarial |

#### Servicios Externos / APIs

| Servicio | Versión | Tipo | Propósito | Justificación |
|----------|---------|------|-----------|---------------|
| **Google Maps API** | v3 | Comercial (Freemium) | Mapas y geolocalización | API estable, amplia documentación, mapas precisos, geocodificación |
| **[Futuro] Stripe/PayPal** | - | Comercial | Pasarela de pagos | Integración de pagos en línea (no en v1.0) |

#### Herramientas de Desarrollo

| Herramienta | Versión | Licencia | Propósito |
|-------------|---------|----------|-----------|
| **Visual Studio** | 2022+ | Community (gratuita) / Professional | IDE principal |
| **Visual Studio Code** | Latest | MIT | Editor de código alternativo |
| **SQL Server Management Studio (SSMS)** | 19+ | Gratuita | Gestión de BD |
| **Git** | 2.40+ | GPL | Control de versiones |
| **GitHub** | - | Freemium | Repositorio remoto, colaboración |
| **Postman** | Latest | Freemium | Testing de APIs |
| **Browser DevTools** | - | - | Debugging frontend |

---

### 2.2.2 Versiones Exactas de Paquetes NuGet Principales

**Paquetes instalados en el proyecto:**

```xml
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
<PackageReference Include="Microsoft.AspNetCore.SignalR" Version="8.0.0" />
<PackageReference Include="Microsoft.Extensions.Http" Version="8.0.0" />
```

---

### 2.2.3 Comparación de Alternativas Tecnológicas

#### Alternativa 1: Backend Framework

| Criterio | ASP.NET Core 8 (Elegido) | Node.js + Express | Django (Python) | Spring Boot (Java) |
|----------|--------------------------|-------------------|-----------------|-------------------|
| **Rendimiento** | ⭐⭐⭐⭐⭐ (Excelente) | ⭐⭐⭐⭐ (Muy bueno) | ⭐⭐⭐ (Bueno) | ⭐⭐⭐⭐ (Muy bueno) |
| **Productividad** | ⭐⭐⭐⭐⭐ (Razor Pages rápido) | ⭐⭐⭐⭐ | ⭐⭐⭐⭐ | ⭐⭐⭐ |
| **Ecosistema** | ⭐⭐⭐⭐⭐ (Microsoft completo) | ⭐⭐⭐⭐⭐ (npm enorme) | ⭐⭐⭐⭐ | ⭐⭐⭐⭐ |
| **Curva de aprendizaje** | ⭐⭐⭐⭐ (Media) | ⭐⭐⭐⭐⭐ (Fácil) | ⭐⭐⭐⭐ (Media) | ⭐⭐⭐ (Alta) |
| **Soporte empresarial** | ⭐⭐⭐⭐⭐ (Microsoft oficial) | ⭐⭐⭐ (Comunidad) | ⭐⭐⭐ (Comunidad) | ⭐⭐⭐⭐ (Oracle/Spring) |
| **Tiempo real (WebSockets)** | ⭐⭐⭐⭐⭐ (SignalR nativo) | ⭐⭐⭐⭐⭐ (Socket.io) | ⭐⭐⭐ (Channels) | ⭐⭐⭐⭐ (WebSocket API) |
| **Decisión** | ✅ **ELEGIDO** | ❌ | ❌ | ❌ |

**Razón de elección:** Familiaridad del equipo con .NET, integración nativa con SQL Server e Identity, alto rendimiento, soporte oficial de Microsoft.

---

#### Alternativa 2: Frontend Framework

| Criterio | Razor Pages (Elegido) | React SPA | Angular SPA | Blazor WebAssembly |
|----------|----------------------|-----------|-------------|--------------------|
| **Productividad inicial** | ⭐⭐⭐⭐⭐ (Rápida) | ⭐⭐⭐ (Configuración) | ⭐⭐ (Compleja) | ⭐⭐⭐⭐ (Media) |
| **Experiencia de usuario** | ⭐⭐⭐ (Tradicional) | ⭐⭐⭐⭐⭐ (SPA fluida) | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐ |
| **SEO** | ⭐⭐⭐⭐⭐ (Server-side) | ⭐⭐⭐ (Requiere SSR) | ⭐⭐⭐ | ⭐⭐⭐ |
| **Complejidad** | ⭐⭐ (Baja) | ⭐⭐⭐⭐ (Alta) | ⭐⭐⭐⭐⭐ (Muy alta) | ⭐⭐⭐⭐ (Alta) |
| **Tiempo de desarrollo** | ⭐⭐⭐⭐⭐ (Rápido) | ⭐⭐⭐ (Medio) | ⭐⭐ (Lento) | ⭐⭐⭐ (Medio) |
| **Tamaño del equipo** | 1-2 devs (Full-stack) | Frontend + Backend | Frontend + Backend | 1-2 devs (Full-stack) |
| **Decisión** | ✅ **ELEGIDO** | ❌ | ❌ | ❌ |

**Razón de elección:** Desarrollo rápido, un solo lenguaje (C#), no requiere equipo frontend separado, suficiente para v1.0 (responsive con Bootstrap).

---

#### Alternativa 3: Base de Datos

| Criterio | SQL Server (Elegido) | PostgreSQL | MySQL | MongoDB |
|----------|---------------------|------------|-------|---------|
| **Robustez empresarial** | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐ | ⭐⭐⭐ |
| **Soporte de Microsoft** | ⭐⭐⭐⭐⭐ | ⭐⭐ (Comunidad) | ⭐⭐ (Oracle) | ⭐⭐ (Comunidad) |
| **Integración con .NET** | ⭐⭐⭐⭐⭐ (Nativa) | ⭐⭐⭐⭐ (Npgsql) | ⭐⭐⭐⭐ (MySql.Data) | ⭐⭐⭐ (Driver) |
| **Herramientas de gestión** | ⭐⭐⭐⭐⭐ (SSMS) | ⭐⭐⭐⭐ (pgAdmin) | ⭐⭐⭐⭐ (Workbench) | ⭐⭐⭐ (Compass) |
| **Costo** | ⭐⭐⭐ (Comercial) | ⭐⭐⭐⭐⭐ (Gratuita) | ⭐⭐⭐⭐⭐ (Gratuita) | ⭐⭐⭐⭐⭐ (Gratuita) |
| **Relacional vs NoSQL** | Relacional (RDBMS) | Relacional | Relacional | NoSQL (Documentos) |
| **Decisión** | ✅ **ELEGIDO** | ❌ | ❌ | ❌ |

**Razón de elección:** Integración perfecta con .NET y Azure, familiaridad del equipo, SSMS robusto, soporte empresarial de Microsoft. Nota: PostgreSQL sería una excelente alternativa gratuita.

---

### 2.2.4 Trade-offs y Decisiones Técnicas

#### Decisión 1: Server-Side Rendering (Razor Pages) vs Client-Side SPA

**Elegido:** Server-Side Rendering (Razor Pages)

**Trade-offs:**

| Aspecto | Server-Side (Razor) | Client-Side (SPA) |
|---------|---------------------|-------------------|
| **Ventajas** | ✅ SEO inmediato<br>✅ Desarrollo rápido<br>✅ Menor complejidad<br>✅ Funciona sin JS | ✅ UX más fluida<br>✅ Menos carga del servidor<br>✅ Interactividad rica |
| **Desventajas** | ❌ Recarga de página<br>❌ Menos interactividad | ❌ SEO complejo<br>❌ Mayor tamaño inicial<br>❌ Requiere JS habilitado |

**Razón:** Para v1.0, la simplicidad y rapidez de desarrollo son más importantes que una UX ultra-fluida. Se complementa con SignalR para tiempo real donde es crítico (geolocalización).

---

#### Decisión 2: Monolito vs Microservicios

**Elegido:** Arquitectura Monolítica (Single Deployment)

**Trade-offs:**

| Aspecto | Monolito | Microservicios |
|---------|----------|----------------|
| **Ventajas** | ✅ Simplicidad<br>✅ Deployment único<br>✅ Debugging fácil<br>✅ Menos overhead | ✅ Escalado independiente<br>✅ Tecnologías heterogéneas<br>✅ Fallos aislados |
| **Desventajas** | ❌ Escalado vertical<br>❌ Coupling mayor | ❌ Complejidad operativa<br>❌ Comunicación entre servicios<br>❌ Debugging difícil |

**Razón:** Para una aplicación de tamaño medio con un equipo pequeño, un monolito bien estructurado es más eficiente. Escalable a microservicios en versiones futuras si es necesario.

---

#### Decisión 3: EF Core vs Dapper vs ADO.NET

**Elegido:** Entity Framework Core

**Trade-offs:**

| Aspecto | EF Core | Dapper | ADO.NET |
|---------|---------|--------|---------|
| **Ventajas** | ✅ Productividad alta<br>✅ Migraciones automáticas<br>✅ LINQ | ✅ Rendimiento excelente<br>✅ Control fino | ✅ Control total<br>✅ Sin overhead |
| **Desventajas** | ❌ Overhead ORM<br>❌ Queries complejas lentas | ❌ No genera DDL<br>❌ Más código manual | ❌ Muy verboso<br>❌ Propenso a errores |

**Razón:** EF Core ofrece el mejor balance entre productividad y rendimiento para aplicaciones CRUD. Para queries críticas se puede usar SQL raw si es necesario.

---

✅ **2.2 STACK TECNOLÓGICO - COMPLETADO**

---

## 2.3 Requerimientos de infraestructura y entornos

### 2.3.1 Hardware Mínimo y Recomendado (On-Premise)

#### Servidor de Aplicación (Web Server)

| Componente | Mínimo | Recomendado | Producción (Alta Disponibilidad) |
|------------|--------|-------------|----------------------------------|
| **CPU** | 2 cores, 2.0 GHz | 4 cores, 2.5 GHz | 8 cores, 3.0 GHz |
| **RAM** | 4 GB | 8 GB | 16 GB |
| **Disco** | 20 GB SSD | 50 GB SSD | 100 GB SSD (RAID 1) |
| **Red** | 100 Mbps | 1 Gbps | 1 Gbps (redundante) |
| **Sistema Operativo** | Windows Server 2019 | Windows Server 2022 | Windows Server 2022 Datacenter |
| **Software** | IIS 10, .NET 8 Runtime | IIS 10, .NET 8 Runtime + SDK | IIS 10, .NET 8 Runtime, monitoreo |

#### Servidor de Base de Datos (SQL Server)

| Componente | Mínimo | Recomendado | Producción (Alta Disponibilidad) |
|------------|--------|-------------|----------------------------------|
| **CPU** | 2 cores, 2.0 GHz | 4 cores, 2.5 GHz | 8 cores, 3.0 GHz |
| **RAM** | 8 GB | 16 GB | 32 GB |
| **Disco** | 50 GB SSD | 100 GB SSD | 500 GB SSD (RAID 10) |
| **Red** | 100 Mbps | 1 Gbps | 1 Gbps (redundante) |
| **Sistema Operativo** | Windows Server 2019 | Windows Server 2022 | Windows Server 2022 Datacenter |
| **Software** | SQL Server 2019 Express | SQL Server 2019 Standard | SQL Server 2019 Enterprise (Always On) |

**Notas:**
- Para desarrollo/pruebas: Servidor único con 8 GB RAM y 100 GB SSD es suficiente
- Para producción: Separar servidores de aplicación y base de datos
- Backups: Disco adicional de al menos 500 GB para respaldos

---

### 2.3.2 Requerimientos Cloud (Azure - Recomendado)

#### Opción 1: Ambiente de Desarrollo/Pruebas

| Servicio | SKU | Especificaciones | Costo Estimado (USD/mes) |
|----------|-----|------------------|--------------------------|
| **Azure App Service** | B1 (Basic) | 1 core, 1.75 GB RAM | ~$55 |
| **Azure SQL Database** | Basic (5 DTU) | 2 GB almacenamiento | ~$5 |
| **Application Insights** | Free tier | 1 GB telemetría/mes | $0 |
| **Dominio** | .azurewebsites.net | Gratuito | $0 |
| **SSL** | Azure Managed Certificate | Gratuito | $0 |
| **Total mensual** | | | **~$60** |

#### Opción 2: Ambiente de Producción

| Servicio | SKU | Especificaciones | Costo Estimado (USD/mes) |
|----------|-----|------------------|--------------------------|
| **Azure App Service** | S1 (Standard) | 1 core, 1.75 GB RAM, auto-scale | ~$75 |
| **Azure SQL Database** | Standard S0 | 10 DTU, 250 GB | ~$30 |
| **Application Insights** | Pay-as-you-go | 5 GB telemetría/mes | ~$10 |
| **Dominio personalizado** | .com + DNS Zone | Registro + hosting DNS | ~$15 |
| **SSL** | Azure Managed Certificate | Gratuito | $0 |
| **Backup Storage** | LRS (50 GB) | Respaldos automáticos | ~$5 |
| **Total mensual** | | | **~$135** |

**Ventajas de Azure:**
- ✅ Escalado automático
- ✅ Backups automáticos
- ✅ Monitoreo integrado (Application Insights)
- ✅ CI/CD con GitHub Actions
- ✅ Sin mantenimiento de hardware
- ✅ SSL gratuito

---

### 2.3.3 Diagrama de Red y Topología de Seguridad

**[INSTRUCCIONES PARA DIAGRAMA]**

```
Título: Topología de Red - Transportes Genesis (Producción)

Zonas:

1. **Zona Pública (DMZ)**
   - Load Balancer / Azure Traffic Manager
   - Firewall / Azure Application Gateway
   - Certificado SSL/TLS
   - IP pública estática

2. **Zona de Aplicación (App Tier)**
   - Servidor Web / Azure App Service
   - IIS + Kestrel
   - SignalR Hub
   - Sin acceso directo desde internet (solo via Load Balancer)

3. **Zona de Datos (Data Tier)**
   - SQL Server / Azure SQL Database
   - Puerto 1433 (solo accesible desde App Tier)
   - Encriptación en tránsito (TLS)
   - Encriptación en reposo (TDE - Transparent Data Encryption)
   - Backups automáticos nocturnos

4. **Zona de Gestión (Management)**
   - Jump Server / Azure Bastion
   - Acceso administrativo
   - VPN o IP whitelisting
   - Multi-Factor Authentication (MFA)

Reglas de Firewall:

- Internet → Load Balancer: Puerto 443 (HTTPS) ✅ Permitido
- Internet → Load Balancer: Puerto 80 (HTTP) → Redirect a 443 ✅
- Load Balancer → App Tier: Puerto 443 ✅ Permitido
- App Tier → Data Tier: Puerto 1433 (SQL) ✅ Permitido
- App Tier → Google Maps API: Puerto 443 (HTTPS) ✅ Permitido
- Internet → App Tier: ❌ Bloqueado (bypass del Load Balancer)
- Internet → Data Tier: ❌ Bloqueado
- Management → Todos: ✅ Permitido (solo desde VPN/IPs autorizadas)

Seguridad:
- WAF (Web Application Firewall) activado
- DDoS Protection Basic (Azure)
- Rate Limiting: 100 requests/min por IP
- Anti-CSRF tokens en formularios
- Content Security Policy (CSP) headers
- HTTPS Strict Transport Security (HSTS)
```

---

### 2.3.4 Entornos de Desarrollo, Pruebas y Producción

#### Ambiente de Desarrollo (Local)

| Componente | Configuración |
|------------|---------------|
| **IDE** | Visual Studio 2022+ Community |
| **Base de Datos** | SQL Server 2019 Express (LocalDB) |
| **Servidor Web** | IIS Express (incluido en Visual Studio) |
| **Puerto** | https://localhost:7XXX |
| **Datos** | Datos de prueba (seeders) |
| **Logs** | Console output, Debug window |

**Comandos de inicio:**
```powershell
# Restaurar paquetes NuGet
dotnet restore

# Aplicar migraciones
dotnet ef database update

# Ejecutar aplicación
dotnet run
```

---

#### Ambiente de Pruebas (Staging)

| Componente | Configuración |
|------------|---------------|
| **Hosting** | Azure App Service (B1) o servidor de pruebas |
| **Base de Datos** | SQL Server (copia de producción con datos anonimizados) |
| **URL** | https://staging.transportesgenesis.com |
| **Datos** | Datos de prueba + datos anonimizados de producción |
| **Logs** | Application Insights |
| **Propósito** | Testing de integración, UAT (User Acceptance Testing) |

**Despliegue:**
- CI/CD automático desde rama `dev` en GitHub
- Tests automatizados antes del deploy
- Notificación al equipo de QA

---

#### Ambiente de Producción

| Componente | Configuración |
|------------|---------------|
| **Hosting** | Azure App Service (S1+) o servidores dedicados |
| **Base de Datos** | SQL Server Standard/Enterprise con Always On |
| **URL** | https://app.transportesgenesis.com |
| **Datos** | Datos reales de clientes |
| **Logs** | Application Insights + Log Analytics |
| **Monitoreo** | Azure Monitor, alertas configuradas |
| **Backups** | Diarios (automáticos), retención 30 días |
| **Disponibilidad** | 99.9% SLA (8.76 horas downtime/año) |

**Despliegue:**
- Manual o CI/CD desde rama `main` con aprobación
- Blue-Green deployment para cero downtime
- Rollback automático si fallan health checks

---

### 2.3.5 Especificación de Contenedores (Docker) - Opcional

Aunque no es obligatorio en v1.0, el sistema puede contenerizarse para facilitar despliegue:

#### Dockerfile (ASP.NET Core App)

```dockerfile
# Imagen base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TransportesGenesis.csproj", "./"]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TransportesGenesis.dll"]
```

#### docker-compose.yml

```yaml
version: '3.8'

services:
  web:
    build: .
    ports:
      - "8080:80"
      - "8443:443"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Server=db;Database=TransportesGenesis;User=sa;Password=YourPassword123!
    depends_on:
      - db
    networks:
      - transportesgenesis-network

  db:
    image: mcr.microsoft.com/mssql/server:2019-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourPassword123!
    ports:
      - "1433:1433"
    volumes:
      - sqldata:/var/opt/mssql
    networks:
      - transportesgenesis-network

volumes:
  sqldata:

networks:
  transportesgenesis-network:
    driver: bridge
```

**Comandos Docker:**
```bash
# Build de imagen
docker build -t transportesgenesis:latest .

# Ejecutar contenedores
docker-compose up -d

# Ver logs
docker-compose logs -f web

# Detener
docker-compose down
```

---

### 2.3.6 CI/CD Pipeline Básico (GitHub Actions)

#### Archivo .github/workflows/deploy.yml (Ejemplo)

```yaml
name: Deploy to Azure

on:
  push:
    branches: [ main ]
  workflow_dispatch:

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest

    steps:
    - uses: actions/checkout@v3

    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '8.0.x'

    - name: Restore dependencies
      run: dotnet restore

    - name: Build
      run: dotnet build --configuration Release --no-restore

    - name: Test
      run: dotnet test --no-restore --verbosity normal

    - name: Publish
      run: dotnet publish -c Release -o ./publish

    - name: Deploy to Azure App Service
      uses: azure/webapps-deploy@v2
      with:
        app-name: 'transportesgenesis-prod'
        publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
        package: ./publish
```

**Flujo:**
1. Push a rama `main` → Trigger automático
2. Checkout del código
3. Build del proyecto
4. Ejecución de tests
5. Publish de artefactos
6. Deploy a Azure App Service
7. Notificación (Slack/Email)

---

✅ **2.3 INFRAESTRUCTURA Y ENTORNOS - COMPLETADO**

---

## 2.4 Diseño de base de datos

### 2.4.1 Esquema Físico - Tablas Principales

El sistema utiliza **dos esquemas** en SQL Server:
- **`genesis`**: Tablas de negocio (buses, rutas, alumnos, pagos, etc.)
- **`dbo`**: Tablas de ASP.NET Core Identity (usuarios, roles, claims)

---

#### Tabla: **genesis.Bus**

```sql
CREATE TABLE genesis.Bus (
    IdBus INT PRIMARY KEY IDENTITY(1,1),
    Placa NVARCHAR(20) NOT NULL UNIQUE,
    Modelo NVARCHAR(100),
    Capacidad INT NOT NULL,
    Activo TINYINT NOT NULL DEFAULT 1,
    FechaRegistro DATETIME2 NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME2 NULL,
    UsuarioRegistro NVARCHAR(450) NULL
);

CREATE INDEX IX_Bus_Placa ON genesis.Bus(Placa);
CREATE INDEX IX_Bus_Activo ON genesis.Bus(Activo) WHERE Activo = 1;
```

**Descripción:** Almacena los buses de la empresa.

---

#### Tabla: **genesis.Ruta**

```sql
CREATE TABLE genesis.Ruta (
    IdRuta INT PRIMARY KEY IDENTITY(1,1),
    IdBus INT NOT NULL,
    Nombre NVARCHAR(200) NOT NULL,
    Descripcion NVARCHAR(500) NULL,
    TipoRuta NVARCHAR(20) NOT NULL, -- 'Mañana' o 'Tarde'
    HoraInicio TIME NOT NULL,
    EsActiva BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    Activo TINYINT NOT NULL DEFAULT 1,
    FechaRegistro DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Ruta_Bus FOREIGN KEY (IdBus) REFERENCES genesis.Bus(IdBus),
    CONSTRAINT CHK_TipoRuta CHECK (TipoRuta IN ('Mañana', 'Tarde'))
);

CREATE INDEX IX_Ruta_IdBus ON genesis.Ruta(IdBus);
CREATE INDEX IX_Ruta_TipoRuta ON genesis.Ruta(TipoRuta);
CREATE INDEX IX_Ruta_EsActiva ON genesis.Ruta(EsActiva) WHERE EsActiva = 1;
```

**Descripción:** Almacena las rutas calculadas para cada bus.

---

#### Tabla: **genesis.Parada**

```sql
CREATE TABLE genesis.Parada (
    IdParada INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(200) NOT NULL,
    Direccion NVARCHAR(500) NULL,
    Latitud DECIMAL(10,7) NOT NULL,
    Longitud DECIMAL(10,7) NOT NULL,
    Activo TINYINT NOT NULL DEFAULT 1,
    FechaRegistro DATETIME2 NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME2 NULL
);

CREATE INDEX IX_Parada_Coordenadas ON genesis.Parada(Latitud, Longitud);
CREATE INDEX IX_Parada_Activo ON genesis.Parada(Activo) WHERE Activo = 1;
```

**Descripción:** Ubicaciones geográficas donde se recogen/dejan alumnos.

---

#### Tabla: **genesis.RutaParada** (Tabla intermedia)

```sql
CREATE TABLE genesis.RutaParada (
    IdRutaParada INT PRIMARY KEY IDENTITY(1,1),
    IdRuta INT NOT NULL,
    IdParada INT NOT NULL,
    Orden INT NOT NULL,
    HoraEstimada TIME NULL,
    Activo TINYINT NOT NULL DEFAULT 1,

    CONSTRAINT FK_RutaParada_Ruta FOREIGN KEY (IdRuta) REFERENCES genesis.Ruta(IdRuta) ON DELETE CASCADE,
    CONSTRAINT FK_RutaParada_Parada FOREIGN KEY (IdParada) REFERENCES genesis.Parada(IdParada),
    CONSTRAINT UQ_RutaParada_RutaOrden UNIQUE (IdRuta, Orden)
);

CREATE INDEX IX_RutaParada_IdRuta ON genesis.RutaParada(IdRuta);
CREATE INDEX IX_RutaParada_Orden ON genesis.RutaParada(IdRuta, Orden);
```

**Descripción:** Relación many-to-many entre Ruta y Parada, con orden secuencial.

---

#### Tabla: **genesis.Alumnos**

```sql
CREATE TABLE genesis.Alumnos (
    IdAlumno INT PRIMARY KEY IDENTITY(1,1),
    NombreCompleto NVARCHAR(200) NOT NULL,
    FechaNacimiento DATE NULL,
    Grado NVARCHAR(50) NULL,
    IdPadre NVARCHAR(450) NOT NULL, -- FK a AspNetUsers
    IdParada INT NULL, -- Parada donde se recoge
    Activo TINYINT NOT NULL DEFAULT 1,
    FechaRegistro DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Alumnos_Padre FOREIGN KEY (IdPadre) REFERENCES dbo.AspNetUsers(Id),
    CONSTRAINT FK_Alumnos_Parada FOREIGN KEY (IdParada) REFERENCES genesis.Parada(IdParada)
);

CREATE INDEX IX_Alumnos_IdPadre ON genesis.Alumnos(IdPadre);
CREATE INDEX IX_Alumnos_IdParada ON genesis.Alumnos(IdParada);
CREATE INDEX IX_Alumnos_Activo ON genesis.Alumnos(Activo) WHERE Activo = 1;
```

**Descripción:** Información de alumnos que usan el servicio de transporte.

---

#### Tabla: **genesis.AsignacionPilotoBus**

```sql
CREATE TABLE genesis.AsignacionPilotoBus (
    IdAsignacion INT PRIMARY KEY IDENTITY(1,1),
    IdUsuarioPiloto NVARCHAR(450) NOT NULL, -- FK a AspNetUsers
    IdBus INT NOT NULL,
    FechaAsignacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    FechaFinAsignacion DATETIME2 NULL,
    EsActual BIT NOT NULL DEFAULT 1,
    Activo TINYINT NOT NULL DEFAULT 1,
    FechaRegistro DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_AsignacionPilotoBus_Usuario FOREIGN KEY (IdUsuarioPiloto) REFERENCES dbo.AspNetUsers(Id),
    CONSTRAINT FK_AsignacionPilotoBus_Bus FOREIGN KEY (IdBus) REFERENCES genesis.Bus(IdBus)
);

CREATE INDEX IX_AsignacionPilotoBus_IdUsuarioPiloto ON genesis.AsignacionPilotoBus(IdUsuarioPiloto);
CREATE INDEX IX_AsignacionPilotoBus_EsActual ON genesis.AsignacionPilotoBus(IdUsuarioPiloto, EsActual) 
    WHERE EsActual = 1;
```

**Descripción:** Asignación de pilotos/monitores a buses (histórico con campo `EsActual`).

---

#### Tabla: **genesis.RegistroRecogida**

```sql
CREATE TABLE genesis.RegistroRecogida (
    IdRegistro INT PRIMARY KEY IDENTITY(1,1),
    IdParada INT NOT NULL,
    IdAlumno INT NOT NULL,
    FechaHoraRecogida DATETIME2 NOT NULL DEFAULT GETDATE(),
    ConfirmadoPor NVARCHAR(450) NULL, -- FK a AspNetUsers (Monitor)
    Latitud DECIMAL(10,7) NULL,
    Longitud DECIMAL(10,7) NULL,
    AlumnoPresente BIT NOT NULL DEFAULT 1,
    Activo TINYINT NOT NULL DEFAULT 1,
    FechaRegistro DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_RegistroRecogida_Parada FOREIGN KEY (IdParada) REFERENCES genesis.Parada(IdParada),
    CONSTRAINT FK_RegistroRecogida_Alumno FOREIGN KEY (IdAlumno) REFERENCES genesis.Alumnos(IdAlumno),
    CONSTRAINT FK_RegistroRecogida_Monitor FOREIGN KEY (ConfirmadoPor) REFERENCES dbo.AspNetUsers(Id)
);

CREATE INDEX IX_RegistroRecogida_IdParada ON genesis.RegistroRecogida(IdParada);
CREATE INDEX IX_RegistroRecogida_IdAlumno ON genesis.RegistroRecogida(IdAlumno);
CREATE INDEX IX_RegistroRecogida_FechaHora ON genesis.RegistroRecogida(FechaHoraRecogida);
CREATE UNIQUE INDEX IX_RegistroRecogida_ParadaAlumno ON genesis.RegistroRecogida(IdParada, IdAlumno, FechaHoraRecogida);
```

**Descripción:** Registros de qué alumnos fueron recogidos en cada parada.

---

#### Tabla: **genesis.ConfirmacionAsistencia**

```sql
CREATE TABLE genesis.ConfirmacionAsistencia (
    IdConfirmacion INT PRIMARY KEY IDENTITY(1,1),
    IdAlumno INT NOT NULL,
    Fecha DATE NOT NULL,
    VaAsistir BIT NOT NULL,
    FechaHoraConfirmacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    Activo TINYINT NOT NULL DEFAULT 1,
    FechaRegistro DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_ConfirmacionAsistencia_Alumno FOREIGN KEY (IdAlumno) REFERENCES genesis.Alumnos(IdAlumno),
    CONSTRAINT UQ_ConfirmacionAsistencia_AlumnoFecha UNIQUE (IdAlumno, Fecha)
);

CREATE INDEX IX_ConfirmacionAsistencia_Fecha ON genesis.ConfirmacionAsistencia(Fecha);
CREATE INDEX IX_ConfirmacionAsistencia_IdAlumno ON genesis.ConfirmacionAsistencia(IdAlumno);
```

**Descripción:** Confirmaciones diarias de asistencia por parte de los padres.

---

#### Tabla: **genesis.Pago**

```sql
CREATE TABLE genesis.Pago (
    IdPago INT PRIMARY KEY IDENTITY(1,1),
    IdPadre NVARCHAR(450) NOT NULL, -- FK a AspNetUsers
    Monto DECIMAL(18,2) NOT NULL,
    FechaPago DATETIME2 NOT NULL DEFAULT GETDATE(),
    MetodoPago NVARCHAR(50) NULL, -- 'Efectivo', 'Transferencia', etc.
    Estado NVARCHAR(20) NOT NULL DEFAULT 'Pendiente', -- 'Pendiente', 'Confirmado'
    Comprobante NVARCHAR(MAX) NULL, -- Ruta de archivo o Base64
    Activo TINYINT NOT NULL DEFAULT 1,
    FechaRegistro DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Pago_Padre FOREIGN KEY (IdPadre) REFERENCES dbo.AspNetUsers(Id),
    CONSTRAINT CHK_Pago_Estado CHECK (Estado IN ('Pendiente', 'Confirmado'))
);

CREATE INDEX IX_Pago_IdPadre ON genesis.Pago(IdPadre);
CREATE INDEX IX_Pago_FechaPago ON genesis.Pago(FechaPago);
CREATE INDEX IX_Pago_Estado ON genesis.Pago(Estado);
```

**Descripción:** Pagos realizados por los padres de familia.

---

### 2.4.2 Diagrama Entidad-Relación (ER)

**[INSTRUCCIONES PARA DIAGRAMA]**

```
Título: Modelo Entidad-Relación - Transportes Genesis

Entidades Principales:

1. AspNetUsers (dbo) [Usuarios del sistema]
   - Id (PK, NVARCHAR(450))
   - UserName
   - Email
   - PasswordHash
   - ...campos de Identity

2. AspNetRoles (dbo) [Roles del sistema]
   - Id (PK, NVARCHAR(450))
   - Name (Administrador, Piloto, Monitor, PadreDeFamilia)

3. Bus (genesis)
   - IdBus (PK)
   - Placa (UNIQUE)
   - Modelo
   - Capacidad
   - Activo

4. Ruta (genesis)
   - IdRuta (PK)
   - IdBus (FK → Bus)
   - Nombre
   - TipoRuta ('Mañana' | 'Tarde')
   - HoraInicio
   - EsActiva

5. Parada (genesis)
   - IdParada (PK)
   - Nombre
   - Direccion
   - Latitud
   - Longitud
   - Activo

6. RutaParada (genesis) [Intermedia]
   - IdRutaParada (PK)
   - IdRuta (FK → Ruta)
   - IdParada (FK → Parada)
   - Orden
   - HoraEstimada

7. Alumnos (genesis)
   - IdAlumno (PK)
   - NombreCompleto
   - FechaNacimiento
   - Grado
   - IdPadre (FK → AspNetUsers)
   - IdParada (FK → Parada)

8. AsignacionPilotoBus (genesis)
   - IdAsignacion (PK)
   - IdUsuarioPiloto (FK → AspNetUsers)
   - IdBus (FK → Bus)
   - FechaAsignacion
   - EsActual

9. RegistroRecogida (genesis)
   - IdRegistro (PK)
   - IdParada (FK → Parada)
   - IdAlumno (FK → Alumnos)
   - FechaHoraRecogida
   - ConfirmadoPor (FK → AspNetUsers)
   - AlumnoPresente

10. ConfirmacionAsistencia (genesis)
    - IdConfirmacion (PK)
    - IdAlumno (FK → Alumnos)
    - Fecha
    - VaAsistir
    - FechaHoraConfirmacion

11. Pago (genesis)
    - IdPago (PK)
    - IdPadre (FK → AspNetUsers)
    - Monto
    - FechaPago
    - MetodoPago
    - Estado

Relaciones (Cardinalidades):

- AspNetUsers (1) ←→ (N) AspNetUserRoles ←→ (N) AspNetRoles [Many-to-Many via tabla intermedia]
- AspNetUsers (1) ←→ (N) Alumnos [Un padre puede tener muchos hijos]
- AspNetUsers (1) ←→ (N) AsignacionPilotoBus [Un piloto puede tener múltiples asignaciones históricas]
- AspNetUsers (1) ←→ (N) RegistroRecogida [Un monitor registra muchas recogidas]
- AspNetUsers (1) ←→ (N) Pago [Un padre realiza muchos pagos]

- Bus (1) ←→ (N) Ruta [Un bus tiene muchas rutas]
- Bus (1) ←→ (N) AsignacionPilotoBus [Un bus puede ser asignado a muchos pilotos]

- Ruta (1) ←→ (N) RutaParada [Una ruta tiene muchas paradas]
- Parada (1) ←→ (N) RutaParada [Una parada puede estar en muchas rutas]

- Parada (1) ←→ (N) Alumnos [Una parada puede tener muchos alumnos asignados]
- Parada (1) ←→ (N) RegistroRecogida [En una parada se registran muchas recogidas]

- Alumnos (1) ←→ (N) RegistroRecogida [Un alumno tiene muchos registros de recogidas]
- Alumnos (1) ←→ (N) ConfirmacionAsistencia [Un alumno tiene muchas confirmaciones]

Notas:
- RutaParada es una tabla asociativa con atributo adicional (Orden, HoraEstimada)
- AsignacionPilotoBus mantiene histórico con campo "EsActual"
- Todas las tablas de negocio heredan campos de auditoría (Activo, FechaRegistro)
```

---

### 2.4.3 Índices y Estrategias de Optimización

#### Índices Principales

| Tabla | Índice | Tipo | Justificación |
|-------|--------|------|---------------|
| **Bus** | `IX_Bus_Placa` | UNIQUE | Búsqueda rápida por placa |
| **Ruta** | `IX_Ruta_IdBus_TipoRuta` | COMPOSITE | Filtrado frecuente por bus y turno |
| **RutaParada** | `IX_RutaParada_IdRuta_Orden` | COMPOSITE | Ordenamiento de paradas en una ruta |
| **AsignacionPilotoBus** | `IX_AsignacionPilotoBus_IdUsuarioPiloto_EsActual` | FILTERED | Búsqueda de asignación activa del piloto |
| **RegistroRecogida** | `IX_RegistroRecogida_ParadaAlumno` | UNIQUE | Prevenir registros duplicados |
| **ConfirmacionAsistencia** | `IX_ConfirmacionAsistencia_AlumnoFecha` | UNIQUE | Una confirmación por alumno por día |

#### Estrategias de Particionamiento (Futuro)

Para tablas que crecen rápidamente:

```sql
-- Particionar RegistroRecogida por año (horizontal partitioning)
CREATE PARTITION FUNCTION PF_RegistroRecogida_Year (DATETIME2)
AS RANGE RIGHT FOR VALUES ('2024-01-01', '2025-01-01', '2026-01-01');

CREATE PARTITION SCHEME PS_RegistroRecogida_Year
AS PARTITION PF_RegistroRecogida_Year ALL TO ([PRIMARY]);
```

**Beneficio:** Mejora de rendimiento en consultas por rango de fechas.

---

### 2.4.4 Estrategia de Migración de Datos (si aplica)

Si existe un **sistema legacy** con datos previos:

#### Fase 1: Análisis

1. Identificar fuentes de datos (Excel, Access, SQL Server antiguo)
2. Mapear campos antiguos → nuevos
3. Detectar inconsistencias (duplicados, formatos)

#### Fase 2: Limpieza (ETL)

```sql
-- Ejemplo: Migrar buses desde tabla antigua
INSERT INTO genesis.Bus (Placa, Modelo, Capacidad, Activo, FechaRegistro)
SELECT 
    UPPER(TRIM(Placa)),
    TRIM(Modelo),
    ISNULL(Capacidad, 40),
    1,
    GETDATE()
FROM SistemaAntiguo.dbo.Buses
WHERE Placa IS NOT NULL;
```

#### Fase 3: Validación

- Comparar conteos de registros
- Verificar integridad referencial
- Auditoría de datos críticos (pagos, alumnos)

#### Fase 4: Cutover

- Backup completo del sistema antiguo
- Migración en horario no laboral (noche/fin de semana)
- Rollback plan preparado

---

✅ **2.4 DISEÑO DE BASE DE DATOS - COMPLETADO**

---

## 2.5 Diseño de APIs e integración

### 2.5.1 Endpoints REST Principales

El sistema expone una **API REST** bajo la ruta base `/api/` para operaciones programáticas.

#### API de Rutas

**GET /api/rutas/bus/{idBus}/activa**

Obtiene la ruta activa del día para un bus específico.

```http
GET /api/rutas/bus/1/activa?tipoRuta=Mañana HTTP/1.1
Host: app.transportesgenesis.com
Authorization: Cookie (Identity)
```

**Respuesta exitosa (200 OK):**
```json
{
  "success": true,
  "message": "Ruta encontrada",
  "data": {
    "idRuta": 123,
    "idBus": 1,
    "placaBus": "ABC-123",
    "nombre": "Ruta Mañana - Zona Norte",
    "tipoRuta": "Mañana",
    "horaInicio": "06:30:00",
    "esActiva": true,
    "paradas": [
      {
        "idParada": 10,
        "nombreParada": "Parada Escuela XYZ",
        "latitud": 14.12345,
        "longitud": -87.98765,
        "orden": 1,
        "horaEstimada": "06:45:00",
        "alumnos": [
          {
            "idAlumno": 50,
            "nombreCompleto": "Juan Pérez",
            "grado": "5to Primaria"
          }
        ]
      }
    ],
    "fechaCreacion": "2025-01-20T10:00:00Z"
  }
}
```

**Respuesta sin ruta (404 Not Found):**
```json
{
  "success": false,
  "message": "No hay ruta calculada para este bus en el turno Mañana",
  "data": null
}
```

---

**POST /api/rutas/calcular**

Calcula una ruta optimizada usando el algoritmo TSP (Traveling Salesman Problem).

```http
POST /api/rutas/calcular HTTP/1.1
Host: app.transportesgenesis.com
Content-Type: application/json
Authorization: Cookie (Identity, Rol: Administrador)

{
  "idBus": 1,
  "fecha": "2025-01-21",
  "tipoRuta": "Mañana",
  "idsAlumnos": [50, 51, 52, 53]
}
```

**Respuesta exitosa (201 Created):**
```json
{
  "success": true,
  "message": "Ruta calculada exitosamente",
  "data": {
    "idRuta": 124,
    "totalParadas": 4,
    "distanciaTotal": 12.5,
    "tiempoEstimado": "45 minutos"
  }
}
```

---

#### API de Pagos

**GET /api/pagos/padre/{idPadre}**

Obtiene el historial de pagos de un padre de familia.

```http
GET /api/pagos/padre/abc123xyz HTTP/1.1
Host: app.transportesgenesis.com
Authorization: Cookie (Identity)
```

**Respuesta exitosa (200 OK):**
```json
{
  "success": true,
  "data": [
    {
      "idPago": 100,
      "monto": 250.00,
      "fechaPago": "2025-01-15T14:30:00Z",
      "metodoPago": "Transferencia",
      "estado": "Confirmado"
    },
    {
      "idPago": 99,
      "monto": 250.00,
      "fechaPago": "2024-12-15T10:00:00Z",
      "metodoPago": "Efectivo",
      "estado": "Confirmado"
    }
  ]
}
```

---

**POST /api/pagos**

Registra un nuevo pago.

```http
POST /api/pagos HTTP/1.1
Host: app.transportesgenesis.com
Content-Type: application/json
Authorization: Cookie (Identity, Rol: PadreDeFamilia)

{
  "idPadre": "abc123xyz",
  "monto": 250.00,
  "metodoPago": "Transferencia",
  "comprobante": "data:image/png;base64,iVBOR..."
}
```

**Respuesta exitosa (201 Created):**
```json
{
  "success": true,
  "message": "Pago registrado exitosamente. Pendiente de confirmación.",
  "data": {
    "idPago": 101,
    "estado": "Pendiente"
  }
}
```

---

#### API de Confirmación de Asistencia

**POST /api/confirmacion-asistencia**

Confirma si un alumno asistirá en una fecha específica.

```http
POST /api/confirmacion-asistencia HTTP/1.1
Host: app.transportesgenesis.com
Content-Type: application/json
Authorization: Cookie (Identity, Rol: PadreDeFamilia)

{
  "idAlumno": 50,
  "fecha": "2025-01-22",
  "vaAsistir": true
}
```

**Respuesta exitosa (200 OK):**
```json
{
  "success": true,
  "message": "Asistencia confirmada para el 22/01/2025"
}
```

---

### 2.5.2 SignalR Hub - Geolocalización en Tiempo Real

#### Hub: **GeolocalizacionHub**

Ubicación: `Hubs/GeolocalizacionHub.cs`

**Métodos del servidor (invocados por clientes):**

```csharp
public async Task EnviarUbicacion(double latitud, double longitud, int idBus)
{
    // Validar usuario autenticado
    var userId = Context.UserIdentifier;

    // Broadcast a todos los clientes conectados al grupo "Mapa"
    await Clients.Group("Mapa").SendAsync(
        "RecibirUbicacion", 
        latitud, 
        longitud, 
        idBus, 
        DateTime.Now
    );

    // Opcional: Guardar en BD para histórico
    // await _context.HistorialUbicaciones.AddAsync(...);
}

public async Task UnirseAlMapa()
{
    await Groups.AddToGroupAsync(Context.ConnectionId, "Mapa");
}

public async Task SalirDelMapa()
{
    await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Mapa");
}
```

**Métodos del cliente (invocados por servidor):**

```javascript
// Cliente JavaScript (en la página del mapa)
connection.on("RecibirUbicacion", function (latitud, longitud, idBus, timestamp) {
    // Actualizar marcador en el mapa
    actualizarMarcadorBus(idBus, latitud, longitud);
    console.log(`Bus ${idBus} en [${latitud}, ${longitud}] a las ${timestamp}`);
});
```

**Flujo de conexión:**

```
1. Cliente se conecta a /geolocalizacionHub
2. Cliente invoca "UnirseAlMapa"
3. Servidor agrega al cliente al grupo "Mapa"
4. Bus envía ubicación cada 10 segundos vía "EnviarUbicacion"
5. Servidor hace broadcast a todos en grupo "Mapa"
6. Clientes reciben "RecibirUbicacion" y actualizan UI
7. Al salir, cliente invoca "SalirDelMapa"
```

---

### 2.5.3 Integración con Servicios Externos

#### Google Maps API

**Propósito:** Visualización de mapas y geocodificación.

**Configuración:**

```csharp
// appsettings.json
{
  "GoogleMaps": {
    "ApiKey": "YOUR_GOOGLE_MAPS_API_KEY_HERE"
  }
}
```

**Uso en JavaScript:**

```html
<script src="https://maps.googleapis.com/maps/api/js?key=YOUR_API_KEY&libraries=places"></script>

<script>
function inicializarMapa() {
    var mapa = new google.maps.Map(document.getElementById('map'), {
        center: {lat: 14.0723, lng: -87.1921}, // Tegucigalpa, Honduras
        zoom: 13
    });

    // Agregar marcador de bus
    var marcador = new google.maps.Marker({
        position: {lat: 14.0723, lng: -87.1921},
        map: mapa,
        title: 'Bus #1'
    });
}
</script>
```

**Endpoints usados:**

- **Maps JavaScript API:** Renderizado de mapas interactivos
- **Geocoding API (opcional):** Convertir direcciones en coordenadas
- **Directions API (futuro):** Calcular rutas óptimas con tráfico en tiempo real

**Límites de uso (Free Tier):**
- 28,000 cargas de mapa / mes gratis
- $7 por cada 1,000 cargas adicionales

---

#### [Futuro] Pasarela de Pagos (Stripe)

**Propósito:** Pagos en línea con tarjeta de crédito/débito.

**Flujo de integración:**

```
1. Padre ingresa datos de tarjeta en formulario
2. Frontend envía datos a Stripe (tokenización)
3. Stripe devuelve token seguro
4. Backend recibe token y procesa cargo
5. Stripe confirma pago
6. Sistema registra pago en BD
7. Usuario recibe confirmación
```

**Ejemplo de código (servidor):**

```csharp
// NuGet: Stripe.net
var options = new ChargeCreateOptions
{
    Amount = 25000, // $250.00 en centavos
    Currency = "usd",
    Source = tokenFromFrontend,
    Description = "Pago mensualidad transporte escolar"
};

var service = new ChargeService();
Charge charge = await service.CreateAsync(options);

if (charge.Status == "succeeded")
{
    // Registrar pago en BD
    var pago = new Pago
    {
        IdPadre = userId,
        Monto = 250.00m,
        MetodoPago = "Tarjeta",
        Estado = "Confirmado"
    };
    _context.Pagos.Add(pago);
    await _context.SaveChangesAsync();
}
```

**Estado:** No implementado en v1.0 (pagos se registran manualmente).

---

### 2.5.4 Contratos de Interfaz (OpenAPI/Swagger)

**Especificación OpenAPI 3.0:**

```yaml
openapi: 3.0.0
info:
  title: Transportes Genesis API
  version: 1.0.0
  description: API REST para gestión de transporte escolar

servers:
  - url: https://app.transportesgenesis.com/api
    description: Servidor de producción

paths:
  /rutas/bus/{idBus}/activa:
    get:
      summary: Obtener ruta activa de un bus
      parameters:
        - name: idBus
          in: path
          required: true
          schema:
            type: integer
        - name: tipoRuta
          in: query
          required: true
          schema:
            type: string
            enum: [Mañana, Tarde]
      responses:
        '200':
          description: Ruta encontrada
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/RutaDto'
        '404':
          description: No hay ruta calculada

components:
  schemas:
    RutaDto:
      type: object
      properties:
        idRuta:
          type: integer
        idBus:
          type: integer
        nombre:
          type: string
        tipoRuta:
          type: string
        paradas:
          type: array
          items:
            $ref: '#/components/schemas/ParadaDto'

    ParadaDto:
      type: object
      properties:
        idParada:
          type: integer
        nombreParada:
          type: string
        latitud:
          type: number
          format: double
        longitud:
          type: number
          format: double
        orden:
          type: integer
```

**Documentación interactiva (Swagger UI):**

Habilitada en desarrollo: `https://localhost:7XXX/swagger`

```csharp
// Startup.cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Transportes Genesis API",
            Version = "v1"
        });
    });
}

public void Configure(IApplicationBuilder app)
{
    if (env.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        });
    }
}
```

---

✅ **2.5 DISEÑO DE APIs E INTEGRACIÓN - COMPLETADO**

---

## 2.6 Patrones de diseño y buenas prácticas

### 2.6.1 Patrones de Diseño Aplicados

#### 1. **Page Model Pattern** (Razor Pages)

**Descripción:** Cada página Razor tiene su propio modelo (`PageModel`) que encapsula la lógica de esa página.

**Ejemplo:**
```csharp
// Pages/Piloto/MiRuta.cshtml.cs
public class MiRutaModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public RutaDto RutaActiva { get; set; }

    public async Task OnGetAsync()
    {
        // Lógica para obtener la ruta
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var asignacion = await _context.AsignacionesPilotoBusDb
            .FirstOrDefaultAsync(a => a.IdUsuarioPiloto == userId && a.EsActual);
        // ...
    }
}
```

**Beneficio:** Separación clara entre vista (CSHTML) y lógica (CS).

---

#### 2. **Dependency Injection (DI)**

**Descripción:** Las dependencias se inyectan vía constructor, no se instancian manualmente.

**Registro en `Startup.cs`:**
```csharp
services.AddScoped<ApplicationDbContext>();
services.AddHttpClient();
services.AddSignalR();
```

**Inyección en constructores:**
```csharp
public MiRutaModel(
    IHttpClientFactory httpClientFactory,
    ApplicationDbContext context)
{
    _httpClientFactory = httpClientFactory;
    _context = context;
}
```

**Beneficio:** Desacoplamiento, facilita testing (mocking).

---

#### 3. **Repository Pattern** (Implícito con EF Core)

**Descripción:** `ApplicationDbContext` actúa como Unit of Work y cada `DbSet<T>` como Repository.

**Ejemplo:**
```csharp
// ApplicationDbContext.cs
public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public DbSet<Bus> BusesDb { get; set; }
    public DbSet<Ruta> RutasDb { get; set; }
    public DbSet<Parada> ParadasDb { get; set; }
    // ...
}

// Uso en PageModel
var buses = await _context.BusesDb.Where(b => b.Activo == 1).ToListAsync();
```

**Beneficio:** Abstracción del acceso a datos, queries con LINQ.

---

#### 4. **DTO (Data Transfer Objects)**

**Descripción:** Objetos para transferir datos entre capas sin exponer entidades de dominio.

**Ejemplo:**
```csharp
// DTOs/Ruta/RutaDto.cs
public class RutaDto
{
    public int IdRuta { get; set; }
    public string Nombre { get; set; }
    public List<ParadaRutaDto> Paradas { get; set; }
}

// Uso: Entidad → DTO
var rutaDto = new RutaDto
{
    IdRuta = ruta.IdRuta,
    Nombre = ruta.Nombre,
    Paradas = ruta.Paradas.Select(p => new ParadaRutaDto { ... }).ToList()
};
```

**Beneficio:** Seguridad (no exponer propiedades internas), flexibilidad.

---

#### 5. **Authorization Pattern**

**Descripción:** Protección de recursos mediante atributos `[Authorize]`.

**Ejemplo:**
```csharp
[Authorize(Roles = "Piloto")]
public class MiRutaModel : PageModel { }

[Authorize(Roles = "Monitor")]
public class RegistrarRecogidasModel : PageModel { }

[AllowAnonymous]
public class LoginModel : PageModel { }
```

**Beneficio:** Seguridad centralizada, fácil de auditar.

---

#### 6. **Factory Pattern** (IHttpClientFactory)

**Descripción:** Creación de instancias de `HttpClient` mediante factory para evitar agotamiento de sockets.

**Ejemplo:**
```csharp
// Registro
services.AddHttpClient();

// Uso
var client = _httpClientFactory.CreateClient();
client.BaseAddress = new Uri($"{Request.Scheme}://{Request.Host}");
var response = await client.GetAsync("/api/rutas/...");
```

**Beneficio:** Gestión eficiente de conexiones HTTP.

---

### 2.6.2 Principios SOLID Aplicados

| Principio | Aplicación en el Proyecto |
|-----------|---------------------------|
| **Single Responsibility** | Cada PageModel tiene una responsabilidad única (ej: `MiRutaModel` solo maneja la vista de ruta) |
| **Open/Closed** | Clases base como `Auditoria` permiten extensión sin modificación |
| **Liskov Substitution** | Clases derivadas de `Auditoria` son intercambiables |
| **Interface Segregation** | Interfaces específicas (ej: `IHttpClientFactory` en lugar de interfaz genérica) |
| **Dependency Inversion** | Dependencias a abstracciones (`IHttpClientFactory`, `DbContext`), no implementaciones concretas |

---

### 2.6.3 Estrategia de Testing

#### Testing Unitario (Unit Tests)

**Framework:** xUnit + Moq

**Ejemplo de test:**
```csharp
public class MiRutaModelTests
{
    [Fact]
    public async Task OnGetAsync_ConAsignacionActiva_RetornaRutaCorrecta()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        var context = new ApplicationDbContext(options);
        var model = new MiRutaModel(null, context);

        // Act
        await model.OnGetAsync();

        // Assert
        Assert.NotNull(model.RutaActiva);
    }
}
```

**Cobertura objetivo:** > 70% de cobertura de código

---

#### Testing de Integración

**Framework:** ASP.NET Core TestServer

**Ejemplo:**
```csharp
public class RutasApiTests : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly HttpClient _client;

    public RutasApiTests(WebApplicationFactory<Startup> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetRutaActiva_RetornaRutaExistente()
    {
        // Act
        var response = await _client.GetAsync("/api/rutas/bus/1/activa?tipoRuta=Mañana");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("\"success\":true", content);
    }
}
```

---

#### Testing End-to-End (E2E)

**Herramienta:** Selenium WebDriver o Playwright

**Ejemplo de escenario:**
```
1. Usuario navega a /Auth/Login
2. Ingresa credenciales de piloto
3. Click en "Iniciar Sesión"
4. Verifica redirección a /Piloto/MiRuta
5. Verifica que aparece "Bus #1" en la página
```

**Estado:** Pendiente de implementación.

---

### 2.6.4 Buenas Prácticas de Código

#### Convenciones de Nomenclatura

```csharp
// PascalCase para clases, métodos, propiedades
public class Bus { }
public void CalcularRuta() { }
public string NombreCompleto { get; set; }

// camelCase para parámetros, variables locales
public void AsignarBus(int idBus, string usuarioPiloto)
{
    var asignacion = new AsignacionPilotoBus();
}

// _camelCase para campos privados
private readonly ApplicationDbContext _context;

// UPPERCASE para constantes
public const string TIPO_RUTA_MANANA = "Mañana";
```

#### Async/Await

```csharp
// ✅ Correcto: métodos asíncronos con sufijo Async
public async Task<RutaDto> OnGetAsync()
{
    var ruta = await _context.RutasDb.FirstOrDefaultAsync();
    return ruta;
}

// ❌ Incorrecto: no usar .Result o .Wait() (bloquea thread)
var ruta = _context.RutasDb.FirstOrDefaultAsync().Result; // NO HACER
```

#### Validación de Entrada

```csharp
// Validar parámetros en controllers/pages
if (string.IsNullOrEmpty(IdPiloto))
{
    MensajeError = "Usuario no identificado";
    return Page();
}

// Usar Data Annotations en modelos
public class LoginViewModel
{
    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
```

#### Logging

```csharp
// Usar ILogger para registrar eventos
private readonly ILogger<MiRutaModel> _logger;

public async Task OnGetAsync()
{
    _logger.LogInformation($"Usuario {IdPiloto} consultó su ruta");

    try
    {
        // ...
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error al obtener ruta");
    }
}
```

---

✅ **2.6 PATRONES Y BUENAS PRÁCTICAS - COMPLETADO**

---

## 2.7 Plan de implementación y roadmap técnico

### 2.7.1 Fases del Proyecto (Completadas)

| Fase | Duración | Módulos Implementados | Estado | Fecha |
|------|----------|----------------------|--------|-------|
| **Fase 0: Setup Inicial** | 1 semana | Configuración de proyecto, BD, Identity | ✅ COMPLETADO | Dic 2024 |
| **Fase 1: MVP Core** | 4 semanas | Login, Roles, Gestión de usuarios | ✅ COMPLETADO | Dic 2024 |
| **Fase 2: Rutas** | 3 semanas | CRUD de buses, paradas, cálculo de rutas (TSP) | ✅ COMPLETADO | Ene 2025 |
| **Fase 3: Geolocalización** | 2 semanas | SignalR Hub, Mapa en tiempo real | ✅ COMPLETADO | Ene 2025 |
| **Fase 4: Pagos y Asistencia** | 3 semanas | Módulo de pagos, Confirmación de asistencia | ✅ COMPLETADO | Ene 2025 |
| **Fase 5: Área de Monitores** | 2 semanas | Registro de recogidas, Área del Monitor | ✅ COMPLETADO | Ene 2025 |
| **Fase 6: Testing y Deploy** | 2 semanas | Testing, Documentación, Despliegue | 🔄 EN CURSO | Ene 2025 |

**Total:** ~17 semanas (4.25 meses)

---

### 2.7.2 Roadmap Futuro (v2.0 y posteriores)

#### v2.0 - Mejoras de UX y Notificaciones (Q2 2025)

| Feature | Prioridad | Esfuerzo | Descripción |
|---------|-----------|----------|-------------|
| **Notificaciones Push** | Alta | 3 semanas | Integración con Firebase/OneSignal para notificar llegada del bus |
| **Chat en tiempo real** | Media | 2 semanas | Mensajería entre padres y administradores |
| **Modo offline** | Baja | 2 semanas | PWA con Service Workers para funcionar sin internet |
| **Temas oscuro/claro** | Baja | 1 semana | Soporte para tema dark mode |

#### v2.5 - Integraciones y Reportes (Q3 2025)

| Feature | Prioridad | Esfuerzo | Descripción |
|---------|-----------|----------|-------------|
| **Integración con Stripe** | Alta | 2 semanas | Pagos en línea con tarjeta |
| **Reportes avanzados (Power BI)** | Media | 3 semanas | Dashboards interactivos con Power BI Embedded |
| **Facturación electrónica** | Media | 3 semanas | Generación de facturas según regulaciones locales |
| **Exportación a Excel/PDF** | Media | 1 semana | Exportar reportes en múltiples formatos |

#### v3.0 - Aplicación Móvil Nativa (Q4 2025)

| Feature | Prioridad | Esfuerzo | Descripción |
|---------|-----------|----------|-------------|
| **App iOS** | Alta | 8 semanas | Aplicación nativa para iPhone/iPad |
| **App Android** | Alta | 8 semanas | Aplicación nativa para Android |
| **Notificaciones nativas** | Alta | 2 semanas | Push notifications en apps móviles |
| **Modo offline avanzado** | Media | 3 semanas | Sincronización de datos offline |

#### v4.0 - Machine Learning y Optimización (Q1 2026)

| Feature | Prioridad | Esfuerzo | Descripción |
|---------|-----------|----------|-------------|
| **Predicción de tiempos** | Media | 4 semanas | ML para predecir tiempos de llegada con tráfico |
| **Optimización dinámica** | Media | 5 semanas | Recalcular rutas en tiempo real según tráfico |
| **Análisis de patrones** | Baja | 3 semanas | Análisis de asistencia, pagos, comportamiento |

---

### 2.7.3 Estrategia de DevOps

#### CI/CD Pipeline (GitHub Actions)

```
┌─────────────────┐
│   Git Push      │
│   (main/dev)    │
└────────┬────────┘
         ↓
┌─────────────────┐
│   Build & Test  │
│   (dotnet test) │
└────────┬────────┘
         ↓
┌─────────────────┐
│   Code Quality  │
│   (SonarQube)   │
└────────┬────────┘
         ↓
┌─────────────────┐
│   Publish       │
│   (dotnet pub)  │
└────────┬────────┘
         ↓
┌─────────────────┐
│   Deploy Azure  │
│   (App Service) │
└────────┬────────┘
         ↓
┌─────────────────┐
│   Health Check  │
│   (Smoke Tests) │
└─────────────────┘
```

#### Monitoreo (Application Insights)

- **Métricas clave:**
  - Request rate (requests/min)
  - Response time (p50, p95, p99)
  - Error rate (%)
  - Active users
  - SignalR connections

- **Alertas configuradas:**
  - Error rate > 5% → Email a equipo
  - Response time > 2s en 5 minutos → Slack
  - Disk space < 10% → SMS
  - SQL DTU > 90% → Email

#### Logging (Serilog + Azure Log Analytics)

```csharp
// Configuración en Startup.cs
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/app.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.AzureAnalytics(workspaceId, authenticationId)
    .CreateLogger();
```

**Niveles de log:**
- **Trace:** Debugging detallado
- **Debug:** Información de desarrollo
- **Information:** Flujo normal de la aplicación
- **Warning:** Situaciones anormales pero manejables
- **Error:** Errores que impiden una operación
- **Critical:** Fallos catastróficos

---

✅ **2.7 PLAN DE IMPLEMENTACIÓN - COMPLETADO**

---

## 2.8 Análisis de riesgos técnicos y plan de mitigación

### 2.8.1 Matriz de Riesgos Técnicos

| ID | Riesgo | Probabilidad | Impacto | Severidad | Mitigación | Contingencia |
|----|--------|--------------|---------|-----------|------------|--------------|
| **RT-01** | Pérdida de conexión SignalR (WebSocket) | **Media (40%)** | **Alto** | 🔴 **Alta** | - Implementar reconexión automática<br>- Fallback a Server-Sent Events<br>- Fallback a Long Polling<br>- Timeout de 30 segundos | - Mostrar mensaje "Reconectando..."<br>- Permitir uso sin tiempo real (solo refresh manual) |
| **RT-02** | Sobrecarga de base de datos con muchos usuarios | **Baja (20%)** | **Alto** | 🟡 **Media** | - Índices optimizados en consultas frecuentes<br>- Caching con Redis (futuro)<br>- Connection pooling habilitado<br>- Query optimization con EF Core | - Escalar verticalmente SQL Server<br>- Implementar Azure SQL Database con auto-scaling<br>- Separar BD de lectura y escritura (CQRS) |
| **RT-03** | Bug en algoritmo TSP con muchas paradas (>30) | **Media (30%)** | **Medio** | 🟡 **Media** | - Tests unitarios exhaustivos<br>- Límite de 30 paradas por ruta<br>- Timeout de 60 segundos en cálculo<br>- Algoritmo heurístico (nearest neighbor) | - Calcular ruta de forma manual<br>- Dividir ruta en sub-rutas<br>- Usar servicios externos (Google Directions API) |
| **RT-04** | Fallo de Google Maps API (cuota excedida) | **Baja (15%)** | **Alto** | 🟡 **Media** | - Monitorear uso de API mensualmente<br>- Cachear mapas estáticos<br>- Optimizar llamadas (lazy loading) | - Usar OpenStreetMap como backup<br>- Mostrar coordenadas sin mapa<br>- Aumentar cuota de Google Maps |
| **RT-05** | Vulnerabilidades de seguridad (XSS, CSRF, SQL Injection) | **Media (30%)** | **Crítico** | 🔴 **Alta** | - Validación de entrada (Data Annotations)<br>- Anti-CSRF tokens automáticos (Razor)<br>- Parametrización de queries (EF Core)<br>- Content Security Policy headers<br>- Auditorías de seguridad trimestrales | - Aplicar parches inmediatamente<br>- Notificar a usuarios<br>- Cambiar contraseñas forzosamente<br>- Contratar auditoría externa |
| **RT-06** | Incompatibilidad con navegadores antiguos | **Baja (10%)** | **Bajo** | 🟢 **Baja** | - Usar Babel/polyfills para JS<br>- Detección de navegador en frontend<br>- Avisar a usuarios sobre requisitos | - Mostrar mensaje: "Actualiza tu navegador"<br>- Versión simplificada para navegadores viejos |
| **RT-07** | Pérdida de datos por fallo de disco | **Muy Baja (5%)** | **Crítico** | 🟡 **Media** | - Backups automáticos diarios<br>- Retención de 30 días<br>- Geo-redundancia (Azure)<br>- RAID 10 en on-premise | - Restaurar desde backup más reciente<br>- Máximo pérdida: 24 horas de datos<br>- Proceso de restauración documentado |
| **RT-08** | Rendimiento degradado en horas pico | **Media (35%)** | **Medio** | 🟡 **Media** | - Load balancer con múltiples instancias<br>- Auto-scaling habilitado<br>- CDN para contenido estático<br>- Output caching en páginas estáticas | - Escalar manualmente más instancias<br>- Priorizar funciones críticas (login, mapa)<br>- Mensaje: "Alta demanda, procesando..." |
| **RT-09** | Errores de migración de base de datos | **Baja (15%)** | **Alto** | 🟡 **Media** | - Revisar migraciones antes de aplicar<br>- Backup antes de cada migración<br>- Probar en staging primero<br>- Rollback script preparado | - Rollback a versión anterior<br>- Aplicar migración manualmente<br>- Hotfix deployment |
| **RT-10** | Dependencia de terceros (Google Maps) sin respuesta | **Muy Baja (5%)** | **Medio** | 🟢 **Baja** | - Circuit Breaker pattern<br>- Timeout de 5 segundos<br>- Retry con backoff exponencial | - Degradar funcionalidad (sin mapa)<br>- Mostrar coordenadas en texto<br>- Notificar al equipo técnico |

**Leyenda:**
- 🔴 **Severidad Alta**: Requiere acción inmediata
- 🟡 **Severidad Media**: Monitorear de cerca
- 🟢 **Severidad Baja**: Aceptable con mitigación básica

---

### 2.8.2 Plan de Respuesta a Incidentes

#### Niveles de Severidad

| Nivel | Criterio | Tiempo de Respuesta | Ejemplo |
|-------|----------|---------------------|---------|
| **P0 - Crítico** | Sistema completamente caído | < 15 minutos | BD inaccesible, servidor caído |
| **P1 - Alto** | Funcionalidad crítica no disponible | < 1 hora | Login no funciona, mapa no carga |
| **P2 - Medio** | Funcionalidad secundaria afectada | < 4 horas | Reportes lentos, emails no enviados |
| **P3 - Bajo** | Bug menor sin impacto operativo | < 24 horas | Formato de fecha incorrecto, typo en texto |

#### Equipo de Respuesta

1. **On-call Engineer:** Monitoreo 24/7 (rotación semanal)
2. **DevOps Lead:** Escalamiento de infraestructura
3. **Database Admin:** Problemas de BD
4. **Product Owner:** Decisiones de negocio

#### Proceso de Escalamiento

```
Incidente detectado (alerta o reporte)
         ↓
On-call Engineer investiga (15 min)
         ↓
¿Puede resolver? → SÍ → Implementa fix → Documenta
         ↓ NO
Escala a DevOps Lead (30 min)
         ↓
¿Requiere cambio mayor? → SÍ → Escala a Product Owner
         ↓ NO
Implementa fix → Testing → Deploy → Postmortem
```

---

### 2.8.3 Estrategia de Backup y Recuperación

#### Backups Automáticos

| Tipo | Frecuencia | Retención | Ubicación | Propósito |
|------|------------|-----------|-----------|-----------|
| **Full Backup** | Diario (2:00 AM) | 30 días | Azure Blob Storage | Recuperación completa |
| **Differential** | Cada 6 horas | 7 días | Azure Blob Storage | Recuperación rápida |
| **Transaction Log** | Cada 15 minutos | 24 horas | Azure Blob Storage | Punto en el tiempo |
| **Código fuente** | Cada commit | Permanente | GitHub | Recuperación de código |

#### Procedimiento de Restauración

**Escenario 1: Pérdida de datos recientes (< 24 horas)**

```sql
-- Restaurar desde backup más reciente
RESTORE DATABASE TransportesGenesis
FROM DISK = 'backup_2025_01_20.bak'
WITH NORECOVERY;

-- Aplicar transaction logs hasta hora específica
RESTORE LOG TransportesGenesis
FROM DISK = 'log_2025_01_20_14_00.trn'
WITH STOPAT = '2025-01-20 13:45:00';
```

**Tiempo estimado:** 30 minutos  
**Pérdida máxima de datos:** 15 minutos

---

**Escenario 2: Corrupción de base de datos**

1. Identificar tablas corruptas
2. Restaurar desde backup en servidor de pruebas
3. Exportar solo datos corruptos
4. Importar en BD de producción
5. Validar integridad referencial

**Tiempo estimado:** 2-4 horas  
**Pérdida máxima de datos:** Ninguna (si backup es reciente)

---

### 2.8.4 Lessons Learned (Post-Mortem)

Después de cada incidente P0 o P1, se debe realizar un post-mortem:

**Plantilla:**

```markdown
# Post-Mortem: [Título del Incidente]

**Fecha:** YYYY-MM-DD
**Duración:** X horas
**Severidad:** P0/P1
**Afectados:** X usuarios

## ¿Qué pasó?
Descripción breve del incidente.

## Línea de Tiempo
- 10:00 AM: Alerta de Application Insights
- 10:05 AM: Investigación inicial
- 10:30 AM: Root cause identificado
- 11:00 AM: Fix aplicado
- 11:15 AM: Sistema estable

## Root Cause
Explicación técnica de la causa raíz.

## ¿Qué salió bien?
- Detección rápida gracias a alertas
- Comunicación efectiva del equipo

## ¿Qué salió mal?
- Faltó monitoreo en X componente
- Documentación desactualizada

## Acción Items
1. [ ] Agregar alertas para X (Responsable: Juan, Fecha: 2025-01-25)
2. [ ] Actualizar documentación (Responsable: María, Fecha: 2025-01-22)
3. [ ] Revisar proceso de deploy (Responsable: Pedro, Fecha: 2025-01-30)
```

---

✅ **2.8 ANÁLISIS DE RIESGOS TÉCNICOS - COMPLETADO**

---

✅ **ESPECIFICACIÓN TÉCNICA COMPLETA - TODAS LAS SECCIONES COMPLETADAS**

---

**FIN DEL DOCUMENTO: 2_Fase_inicial_ESPECIFICACION_TECNICA_v1.md**
