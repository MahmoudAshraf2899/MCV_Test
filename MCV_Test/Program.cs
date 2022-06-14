using MCV_Test.Models;
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

builder.Services.AddMvc(options =>
{
    options.EnableEndpointRouting = false;



}); 
//Complete Serilog Configuration.
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.File("..\\MCV_Test\\Logs\\Logs.txt")); //This Path Should Exist With Path in (appSettings.json)

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions
                .ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));

});
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
//app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MCV Test v1"));
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MCV Test v1");
    c.RoutePrefix = string.Empty;
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

}


app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("CorsPolicy");


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});
app.Run();
