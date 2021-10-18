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
app.MapGet("/person", () => new Person("Jon", "Doe"));
app.MapGet("/csharp/nestedproperties", () =>
{
    List<Status> statuses = new()
    {
        new(Category.Normal, new(20, false, 20)),
        new(Category.Warning, new(20, false, 60)),
        new(Category.Danger, new(20, true, 60)),
        new(Category.Danger, new(100, false, 20)),
        new(Category.Unknown, new(100, false, 20))
    };

    var messages = new List<string>();
    foreach (Status status in statuses)
    {
        string message = status switch
        {
            { Category: Category.Normal } => "Let the good times roll",
            { Category: Category.Warning, Reading.PM25: > 50 and < 100 } => "Check the air filters",
            { Reading.PM25: > 200 } => "There must be a fire somewhere. Don't go outside.",
            { Reading.SmokeDetected: true } => "We have a fire!",
            { Category: Category.Danger } => "Something is badly wrong",
            _ => "Unknown status"
        };

        messages.Add(message);
    }

    return messages;
});

app.UseSwaggerUI();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

await app.RunAsync();

record struct Reading(int Temperature, bool SmokeDetected, int PM25);
record struct Status(Category Category, Reading Reading);
enum Category
{
    Normal,
    Warning,
    Danger,
    Unknown
}

public record Person(string FirstName, string LastName);


