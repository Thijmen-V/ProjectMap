using Microsoft.AspNetCore.Identity;
using ProjectMap.WebApi.Repositories;
using ProjectMap.WebApi.Models;
using ProjectMap.WebApi.Controllers;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//builder.Services.AddSingleton<IEnvironmentRepository, EnvironmentRepository>();

var sqlConnectionString = builder.Configuration.GetValue<string>("SqlConnectionString");
var sqlConnectionStringFound = !string.IsNullOrWhiteSpace(sqlConnectionString);

builder.Services.AddTransient<IEnvironmentRepository, EnvironmentRepository>(o => new EnvironmentRepository(sqlConnectionString));

builder.Services.AddTransient<IObjectRepository, ObjectRepository>(o => new ObjectRepository(sqlConnectionString));

// Adding the HTTP Context accessor to be injected. This is needed by the AspNetIdentityUserRepository
// to resolve the current user.
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IAuthenticationService, AspNetIdentityAuthenticationService>();

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 10;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = true;
})
.AddRoles<IdentityRole>()
.AddDapperStores(options =>
{
    options.ConnectionString = sqlConnectionString;
});

var app = builder.Build();

// Build timestamp implementation
var buildTimestamp = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
app.MapGet("/", () =>
    $"The API is up 🚀\n" +
    $"Connection string found: {(sqlConnectionStringFound ? "✅" : "❌")}\n" +
    $"Build timestamp: {buildTimestamp}");

app.UseAuthorization();
app.MapGroup("/account").MapIdentityApi<IdentityUser>();
app.MapControllers().RequireAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


////Program

//using Microsoft.AspNetCore.Identity;
//using Microsoft.Identity.Client;
//using ProjectMap.WebApi;
//using ProjectMap.WebApi.Services;



//var builder = WebApplication.CreateBuilder(args);


////var sqlConnectionString = builder.Configuration["SqlConnectionString"];
//var sqlConnectionString = builder.Configuration.GetValue<string>("SqlConnectionString");
//var sqlConnectionStringFound = !string.IsNullOrWhiteSpace(sqlConnectionString);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

//builder.Services.Configure<RouteOptions>(o => o.LowercaseUrls = true);

//builder.Services.AddHttpContextAccessor();
//builder.Services.AddTransient<IAuthenticationService, AspNetIdentityAuthenticationService>();

////builder.Services.AddSingleton<IEnvironmentRepository, EnvironmentRepository>();


//builder.Services.AddAuthentication();
//builder.Services
//    .AddIdentityApiEndpoints<IdentityUser>(options =>
//    {
//        options.User.RequireUniqueEmail = true;
//        // ... andere opties ...
//        options.Password.RequiredLength = 10;
//    })
//    .AddRoles<IdentityRole>()
//    .AddDapperStores(options =>
//    {
//        options.ConnectionString = sqlConnectionString;
//    });

////if (string.IsNullOrWhiteSpace(sqlConnectionString))
////    throw new InvalidProgramException("Configuration variable SqlConnectionString not found");

//builder.Services.AddTransient<IEnvironmentRepository, EnvironmentRepository>(o => new EnvironmentRepository(sqlConnectionString));
//builder.Services.AddTransient<IObjectRepository, ObjectRepository>(o => new ObjectRepository(sqlConnectionString));
//builder.Services.AddTransient<IAuthenticationService, AspNetIdentityAuthenticationService>();

//var app = builder.Build();

//app.MapGet("/", () => $"The API is up 🚀. Connection string found: {(sqlConnectionStringFound ? "✅" : "❌")}");
////Configure the HTTP request pipeline.|
//app.MapOpenApi();

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapGroup("/account").MapIdentityApi<IdentityUser>();

//app.MapControllers().RequireAuthorization();

//app.Run();