using NotificationAPI.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SignalR configuration
builder.Services.AddSignalR();

// Add CORS policy to allow requests from the frontend application
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed(_ => true); // Allow all origins (for testing)
            /*.WithOrigins("https://localhost:7087")*/ // hoặc domain thật
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(); // Enable CORS trước MapControllers và MapHub

app.MapControllers();

// Map SignalR hubs
app.MapHub<NotificationHub>("/notificationHub");

app.Run();
