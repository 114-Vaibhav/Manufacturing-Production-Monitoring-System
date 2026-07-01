using Microsoft.EntityFrameworkCore;
using backend.DataAccessLayer;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using API.Middleware;
using API.BackgroundServices;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization; // Make sure to add this using statement at the top

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<MPMSDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Dependency Injection
builder.Services.AddScoped(
    typeof(IRepository<,>),
    typeof(Repository<,>)
);

builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IMachineServices, MachineServices>();
builder.Services.AddScoped<IMachineReadingServices, MachineReadingServices>();
builder.Services.AddScoped<IMaintenanceLogServices, MaintenanceLogServices>();
builder.Services.AddScoped<IDefectServices, DefectServices>();
builder.Services.AddScoped<IProductServices, ProductServices>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IProductionPlanServices, ProductionPlanServices>();
builder.Services.AddScoped<IProductionRecordServices, ProductionRecordServices>();
builder.Services.AddScoped<IProductionOrderServices, ProductionOrderServices>();
builder.Services.AddScoped<IProductionAnalyticsServices, ProductionAnalyticsServices>();
builder.Services.AddScoped<IAuditLogService, DbAuditLogService>();
builder.Services.AddScoped<IIoTSensorService, IoTSensorService>();
builder.Services.AddHostedService<MachineSensorSimulatorService>();
builder.Services.AddHostedService<ProductionAnalyticsWorker>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    // This forces the framework to pass the request to your controller
    // even if the model binding isn't perfect, allowing YOUR code to handle the error.
    options.SuppressModelStateInvalidFilter = true;
});

// Update your AddControllers configuration:
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // This tells C# to automatically handle string-to-enum conversions
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

#region Authentication & Authorization

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],

            ValidateAudience = false,

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!)),

            RoleClaimType = ClaimTypes.Role,
            NameClaimType = ClaimTypes.Name
        };
    });

builder.Services.AddAuthorization();

#endregion

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Swagger Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Global Exception Middleware
app.UseMiddleware<ExceptionMiddleware>();

// app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
