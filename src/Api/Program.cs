using System.Text;
using Ecommerce.Domain;
using Ecommerce.Application;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Ecommerce.Application.Features.Products.Queries.GetProductList;
using MediatR;
using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Infrastructure.ImageCloudinary;
using System.Text.Json.Serialization;
using Ecommerce.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddDbContext<EcommerceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"),
    b => b.MigrationsAssembly(typeof(EcommerceDbContext).Assembly.FullName)); //permite que se ejecute las sintaxis sql en consola
});

// Add services to the container.
builder.Services.AddMediatR(typeof(GetProductListQueryHandler).Assembly);
//Para el manejo de las imagenes
builder.Services.AddScoped<IManageImageService, ManageImageService>();


//Obliga a que todos los endpoints esten protegidos
builder.Services.AddControllers( op =>{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    op.Filters.Add(new AuthorizeFilter(policy));

})
.AddJsonOptions(op =>
{
    //Ignoramos las referencias circulares
    op.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

//Habilitamos el uso de la seguridad del identity
IdentityBuilder identityBuilder = builder.Services.AddIdentityCore<Usuario>();
//Sobreescribimos el servicio para obtener acceso al servicio
identityBuilder = new IdentityBuilder(identityBuilder.UserType,identityBuilder.Services);
//Sobreescribimos para que se pueda acceder al rol desde el payload del token
identityBuilder.AddRoles<IdentityRole>().AddDefaultTokenProviders().AddEntityFrameworkStores<EcommerceDbContext>();
//Agregamos los claims por defecto
identityBuilder.AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<Usuario,IdentityRole>>();
//Soporte del identity builder dentro de la base de datos.
identityBuilder.AddEntityFrameworkStores<EcommerceDbContext>();
//soporte de las tareas del login
identityBuilder.AddSignInManager<SignInManager<Usuario>>();
//agregamos el soporte para la creacion de los take times para nuevo recordo de usuario o rol
builder.Services.TryAddSingleton<ISystemClock, SystemClock>();

//Declaramos la configuracion del token
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(op =>
{
    op.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,    //se tiene que usar el objeto key
        IssuerSigningKey = key,             //objeto clave desencriptar token
        ValidateAudience = false,
        ValidateIssuer = false
    };
});

//Agregamos la politica de autorizacion mediante CORS
builder.Services.AddCors(op =>
{
    op.AddPolicy("CorsPolicy", builder =>
    {
        builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

//que llame a la politica de autorizacion de CORS
app.UseCors("CorsPolicy");

app.MapControllers();


using(var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var loggerFactory = service.GetRequiredService<ILoggerFactory>();

    try
    {
        //Seccion que crea las tablas
        var context = service.GetRequiredService<EcommerceDbContext>();
        var usuarioManager = service.GetRequiredService<UserManager<Usuario>>();
        var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
        await context.Database.MigrateAsync();

        //poblamos las tablas con los datos base
        await EcommerceDbContextData.LoadDataAsync(context,usuarioManager,roleManager,loggerFactory);
    }
    catch (Exception ex)
    {
         // TODO
         var logger = loggerFactory.CreateLogger<Program>();
         logger.LogError(ex, "Error en la migracion");
    }


}


app.Run();
