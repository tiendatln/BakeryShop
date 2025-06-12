using AutoMapper;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using UserAPI.Data;
using UserAPI.Mapper;
using UserAPI.Model;
using UserAPI.Repository;
using UserAPI.Repository.Interface;
using UserAPI.Service;
using UserAPI.Service.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// 1. Build EDM model
IEdmModel GetEdmModel()
{
    var odataBuilder = new ODataConventionModelBuilder();
    odataBuilder.EntitySet<User>("Users");

    return odataBuilder.GetEdmModel();
}

// 2. Add OData services
builder.Services.AddControllers().AddOData(options =>
{
    options
        //.AddRouteComponents("odata", GetEdmModel()) // route: /odata/Users
        .Select()
        .Filter()
        .OrderBy()
        .Expand()
        .Count()
        .SetMaxTop(100);
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inject mapper
builder.Services.AddAutoMapper(typeof(UserMapper));

// Inject DB
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Inject Repo
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Inject Service
builder.Services.AddScoped<IUserService, UserService>();

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
