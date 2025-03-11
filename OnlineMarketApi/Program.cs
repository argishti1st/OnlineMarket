using Elastic.CommonSchema;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OnlineMarket.Api.Configurations;
using OnlineMarket.Api.Middlewares;
using OnlineMarket.Application.Features.Interfaces;
using OnlineMarket.Application.Features.Validators;
using OnlineMarket.Application.Users.Commands;
using OnlineMarket.Infrastructure.Data;
using OnlineMarket.Infrastructure.Data.Identity;
using OnlineMarket.Infrastructure.Repositiories;
using Serilog;
using Serilog.Extensions.Hosting;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthenticationService(builder.Configuration);
builder.Services.AddAuthorization();

builder.Host.UseSerilog();

builder.Services.ConfigureLogging(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommandHandler).Assembly));
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

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
