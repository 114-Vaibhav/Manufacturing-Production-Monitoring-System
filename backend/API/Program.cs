using Microsoft.EntityFrameworkCore;
using backend.DataAccessLayer;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using BusinessLayer.Interfaces;
using BusinessLayer.Services;

using API.Middleware;

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
// builder.Services.AddScoped<IMachineServices, MachineServices>();
// builder.Services.AddScoped<IDefectServices, DefectServices>();
// builder.Services.AddScoped<IProductServices, ProductServices>();
builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

// Swagger Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Global Exception Middleware
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
