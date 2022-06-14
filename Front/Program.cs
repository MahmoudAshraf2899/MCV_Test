using Front.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
// Using Serilog For Loggin                
var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .CreateLogger();

try
{
    Log.Information("Application Starting ...");


}
catch (Exception ex)
{
    Log.Fatal(ex, "The Application Failed To Start !");

}
finally
{
    Log.CloseAndFlush();
}
//Using Serilog For Loggin    
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));

});
//Complete Serilog Configuration. 

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.File("..\\Front\\Logs\\Logs.txt")); //This Path Should Exist With Path in (appSettings.json)


var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseDefaultFiles();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
