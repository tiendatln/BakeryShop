using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Load ocelot.json
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot();

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],

            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,

            // ✅ Cấu hình này đảm bảo Ocelot đọc được đúng claim gốc
            NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
        };

        // ✅ Bắt buộc: Không cho .NET tự ánh xạ claim
        options.MapInboundClaims = false;
    });

// Authorization policies
builder.Services.AddAuthorization();

// Add HttpClient for making requests to other services

builder.Services.AddHttpClient("AuthAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7009/api/Auth/"); // Adjust the base address as needed
});

builder.Services.AddHttpClient("UserAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7227/api/Users/"); // Adjust the base address as needed
});

builder.Services.AddHttpClient("ProductAndCategoryAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7016/api/Products/"); // Adjust the base address as needed
});

builder.Services.AddHttpClient("CartAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7027/api/Cart/"); // Adjust the base address as needed
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Ensure this is before UseAuthorization
app.UseAuthorization(); // Ensure this is after UseAuthentication

app.MapControllers();

app.Use(async (context, next) =>
{
    Console.WriteLine("👀 CLAIMS:");
    foreach (var claim in context.User.Claims)
    {
        Console.WriteLine($"👉 {claim.Type} = {claim.Value}");
    }
    await next();
});
// Use Ocelot middleware
await app.UseOcelot();

app.Run();
