using AuthAPI.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inject HttpClient for external API calls
builder.Services.AddHttpClient("UserAPI", client =>
{
    client.BaseAddress = new Uri("https://userapi-z1zs.onrender.com/api/Users/"); // Adjust the base address as needed
});

// Inject Service
builder.Services.AddScoped<AuthAPI.Service.Interface.IUserValidationService, AuthAPI.Service.UserValidationService>();

builder.Services.AddScoped<TokenService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
