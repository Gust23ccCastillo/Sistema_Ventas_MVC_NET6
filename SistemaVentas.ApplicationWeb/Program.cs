
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SistemaVentas.ApplicationWeb.Profits.Automapper;
using SistemaVentas.Dal;
using SistemaVentas.Entities;
using SistemaVentas.IOC;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();


//SERVICIO CREADO EN LA IOC PARA IMPLEMENTAR EN ESA CAPA EN SU CLASE
builder.Services.InjectDependency(builder.Configuration);

//INJECTAR SERVICIO DE AUTOMAPPER
builder.Services.AddAutoMapper(typeof(CategoryAndCategoryDtoMapper));

////CONFIGURACION DE IDENTITY
//builder.Services.AddIdentity<ApplicationUserIdentity, IdentityRole>(options =>
//{
//    options.Password.RequireDigit = true;
//    options.Password.RequireLowercase = true;
//    options.Password.RequireUppercase = true;
//    options.Password.RequiredLength = 6;
//    options.Password.RequireNonAlphanumeric = false;
    
//})
//    .AddEntityFrameworkStores<DataBase_Application_Sales_MVCContext>()
//    .AddDefaultTokenProviders();

//// CONFIGURACION DE AUTHENTICATION POR JWT Y COOKIES
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddCookie()
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        ValidAudience = builder.Configuration["Jwt:Issuer"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//    };
//});

//// Configura la autorización por roles
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
//    options.AddPolicy("EmployeePolicy", policy => policy.RequireRole("Employee"));
//});

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
