var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

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
