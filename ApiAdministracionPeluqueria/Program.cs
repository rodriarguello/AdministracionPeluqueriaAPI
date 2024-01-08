using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Utilidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using ApiAdministracionPeluqueria.Services.Interfaces;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.AlergiaDTO;
using ApiAdministracionPeluqueria.Services;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.EnfermedadDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.RazaDTO;

var builder = WebApplication.CreateBuilder(args);

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles).AddNewtonsoftJson(x=>x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);//Config Ignorar Ciclos

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

//string connectionString = builder.Configuration.GetConnectionString("mySql");
//var serverVersion = new MySqlServerVersion(new Version(8,0,33));


//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(connectionString,serverVersion));

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("conexionSql")));

#region CONFIGURACION JWT

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

    .AddJwtBearer(opciones =>

    opciones.TokenValidationParameters = new TokenValidationParameters
    {

        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["llaveJwt"])),
        ClockSkew = TimeSpan.Zero

    }) ;
#endregion


builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {

        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header

    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {

            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                Type  = ReferenceType.SecurityScheme,
                Id = "Bearer"

                }
            },

            new string[]{}

        }

    });


});



builder.Services.AddCors(opciones=> {

    opciones.AddPolicy("Free", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });

});



builder.Services.AddAutoMapper(typeof(AutoMapperProfile));


builder.Services.AddIdentity<Usuario, IdentityRole>( options=> options.Password.RequireNonAlphanumeric = false).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddTransient<ResponseApi>();

builder.Services.AddScoped<DbInicializador>();
builder.Services.AddScoped<IGenericService<AlergiaCreacionDTO,AlergiaDTO>,AlergiaService>();
builder.Services.AddScoped<IGenericService<EnfermedadCreacionDTO,EnfermedadDTO>, EnfermedadService>();
builder.Services.AddScoped<IGenericService<RazaCreacionDTO,RazaDTO>,RazaService>();
builder.Services.AddScoped<ICuentaService,CuentaService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<ITokenService,TokenService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("Free");
app.UseHttpsRedirection();


app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();

    try
    {
        var dbInicializador = services.GetRequiredService<DbInicializador>();

        dbInicializador.InicializarDb();
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();

        logger.LogError(ex, "Error al realizar las migraciones");
    }
}

app.MapControllers();

app.Run();
