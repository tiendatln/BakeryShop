using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using ProductAndCategoryAPI.Data;
using ProductAndCategoryAPI.DTOs;
using ProductAndCategoryAPI.Models;
using ProductAndCategoryAPI.Repositories;
using ProductAndCategoryAPI.Service;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var odataBuilder = new ODataConventionModelBuilder();
odataBuilder.EntitySet<ReadProductDTO>("Products")
    .EntityType.HasKey(p => p.ProductID); // Chỉ định key cho entity
odataBuilder.EntitySet<ReadCategoryDTO>("Categories")
    .EntityType.HasKey(c => c.CategoryID);
builder.Services.AddControllers().AddOData(options => options
.AddRouteComponents("odata", odataBuilder.GetEdmModel()).Select().Filter().OrderBy().Expand().Count().SetMaxTop(100));
odataBuilder.EntitySet<Product>("OdataProduct");

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
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

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
