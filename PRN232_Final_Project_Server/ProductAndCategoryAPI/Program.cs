using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using ProductAndCategoryAPI.Data;
using ProductAndCategoryAPI.DTOs;
using ProductAndCategoryAPI.Repositories;
using ProductAndCategoryAPI.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var odataBuilder = new ODataConventionModelBuilder();
odataBuilder.EntitySet<ReadProductDTO>("Products")
    .EntityType.HasKey(p => p.ProductID); // Chỉ định key cho entity

builder.Services.AddControllers().AddOData( options => options
.AddRouteComponents("odata",odataBuilder.GetEdmModel()).Select().Filter().OrderBy().Expand().Count().SetMaxTop(100));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IProductRespository, ProductRespository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddDbContext<ProductAndCategoryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



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
