using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddHttpLogging(logging =>
{
    // Customize HTTP logging here.
    // logging.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders |
    // HttpLoggingFields.RequestBody;
    logging.LoggingFields = HttpLoggingFields.All;
    // logging.RequestHeaders.Add("My-Request-Header");
    logging.ResponseHeaders.Add("My-Response-Header");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseHttpLogging();
app.UseSwagger();
app.MapGet("/", () => "Hello World 11!");
app.MapGet("/person", () => new Person("Aleksey", "Pyatlin"));

app.UseSwaggerUI();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

await app.RunAsync();

public record Person(string FirstName, string LastName);
