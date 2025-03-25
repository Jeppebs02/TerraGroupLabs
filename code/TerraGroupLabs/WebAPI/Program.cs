var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add configuration from appsettings.json and environment variables
builder.Configuration.AddJsonFile("/Configs/appsettings.json", optional: false, reloadOnChange: true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Swagger configuration removed
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
