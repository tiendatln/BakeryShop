using CartAPI.Data;
using CartAPI.DTOs;
using CartAPI.Profiles;
using CartAPI.Repositories;
using CartAPI.Services;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// Add AutoMapper
builder.Services.AddAutoMapper(typeof(CartPoriles));

// Add DbContext
builder.Services.AddDbContext<CartDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection cho Repository và Service
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();

var oDataBuilder = new ODataConventionModelBuilder();
oDataBuilder.EntitySet<CartDTO>("Cart");

builder.Services.AddControllers().AddOData(options =>
    options.AddRouteComponents("odata", oDataBuilder.GetEdmModel())
           .SetMaxTop(10)
           .Count()
           .Filter()
           .OrderBy()
           .Expand()
           .Select());


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
