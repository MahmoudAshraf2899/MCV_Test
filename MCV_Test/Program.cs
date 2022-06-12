using MCV_Test.Models;
using Microsoft.EntityFrameworkCore;


string myAllowsSpecificOrigin = "_myAllowSpecificOrigin";

var builder = WebApplication.CreateBuilder(args);

/*
   builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("https://localhost:44362")
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });
 */
builder.Services.AddCors(options =>
{
    options.AddPolicy("_myAllowSpecificOrigin",
        builder =>
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithOrigins()
        );
});
builder.Services.AddMvc(options =>
{
    options.EnableEndpointRouting = false;



});
//builder.Services.AddMvc(option => option.EnableEndpointRouting = false);

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

app.UseCors(myAllowsSpecificOrigin);


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});
app.Run();
