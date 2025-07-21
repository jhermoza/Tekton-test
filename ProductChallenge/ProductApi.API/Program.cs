using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.OpenApi.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using ProductApi.API.Middleware;
using ProductApi.Application.Interfaces;
using ProductApi.Infrastructure.Repositories;
using ProductApi.Infrastructure.Services;
using ProductApi.API.Validators;
using ProductApi.Infrastructure.Data;
using ProductApi.Infrastructure.Options;
using ProductApi.API.Mapping;
using ProductApi.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

// Dedicated logger for request timing
var timingLogger = new LoggerConfiguration()
    .WriteTo.File("logs/request-timing-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Services.AddSingleton<Serilog.ILogger>(timingLogger);

// Register application services
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<ProductRequestValidator>();
builder.Services.AddMemoryCache();
builder.Services.AddAutoMapper(typeof(ProductProfile));
builder.Services.Configure<DiscountApiOptions>(
    builder.Configuration.GetSection("DiscountApi"));
builder.Services.AddHttpClient<IDiscountService, DiscountService>();
builder.Services.AddScoped<IStatusCache, StatusCache>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product API", Version = "v1" });
});

var app = builder.Build();

// Serilog request timing middleware
app.UseMiddleware<RequestTimingSerilogMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
