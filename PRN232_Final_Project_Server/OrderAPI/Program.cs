using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using OrderAPI.Data;
using OrderAPI.DTOs;
using OrderAPI.Models;
using OrderAPI.Profiles;
using OrderAPI.Repositories;
using OrderAPI.Repositories.Interfaces;
using OrderAPI.Services;
using OrderAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(OrderProfile));

// Add services to the container.

builder.Services.AddControllers()
    .AddOData(opt => opt
        .Select()
        .Filter()
        .Expand()
        .OrderBy()
        .SetMaxTop(100)
        .Count()
        .AddRouteComponents("odata", GetEdmModel()) 
    );

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<OrderDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IOrderRepo, OrderRepo>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderDetailRepo, OrderDetailRepo>();

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

IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();

    // Orders
    var orderEntity = builder.EntitySet<ReadOrderDTO>("Orders").EntityType;
    orderEntity.HasKey(o => o.OrderID);

    // OrderDetails
    var orderDetailEntity = builder.EntitySet<ReadOrderDetailDTO>("OrderDetails").EntityType;
    orderDetailEntity.HasKey(od => od.OrderDetailID);

    return builder.GetEdmModel();
}
