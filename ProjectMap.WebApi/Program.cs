using Microsoft.Identity.Client;
using ProjectMap.WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.Configure<RouteOptions>(o => o.LowercaseUrls = true);

//builder.Services.AddSingleton<IEnvironmentRepository, EnvironmentRepository>();

var sqlConnectionString = builder.Configuration["SqlConnectionString"];

if (string.IsNullOrWhiteSpace(sqlConnectionString))
    throw new InvalidProgramException("Configuration variable SqlConnectionString not found");

builder.Services.AddTransient<IEnvironmentRepository, EnvironmentRepository>(o => new EnvironmentRepository(sqlConnectionString));
builder.Services.AddTransient<IObjectRepository, ObjectRepository>(o => new ObjectRepository(sqlConnectionString));


var app = builder.Build();

// Configure the HTTP request pipeline.|
app.MapOpenApi();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();