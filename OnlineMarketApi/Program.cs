using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnlineMarket.Api.Configurations;
using OnlineMarket.Api.Middlewares;
using OnlineMarket.Application.Features.Products.Commands;
using OnlineMarket.Application.Features.Validators;
using OnlineMarket.Application.Interfaces;
using OnlineMarket.Infrastructure.Data;
using OnlineMarket.Infrastructure.Data.Identity;
using OnlineMarket.Infrastructure.Repositiories;
using Serilog;
using System.Reflection;
using System.Reflection.Metadata;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthenticationService(builder.Configuration);
builder.Services.AddScoped<IdentityDataSeeder>();

builder.Host.UseSerilog();

builder.Services.ConfigureLogging(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly));
builder.Services.AddMediatR(config =>
{
    Assembly[] handlersAssemblies = new[] { typeof(AssemblyReference).GetTypeInfo().Assembly };
    config.RegisterServicesFromAssemblies(handlersAssemblies)
    .AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssemblyContaining<CreateProductValidator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    try
    {
        dbContext.Database.Migrate();
        Serilog.Log.Information("Successfully migrated the database.");
    }
    catch (Exception ex)
    {
        Serilog.Log.Error(ex, "An error occurred while migrating the database.");
    }
}

app.UseSerilogRequestLogging();
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IdentityDataSeeder>();
    await seeder.SeedAsync(scope.ServiceProvider);
}

app.Run();
