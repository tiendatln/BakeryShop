using AutoMapper;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System.Text;
using UserAPI.Data;
using UserAPI.DTOs;
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

    // Vẫn đăng ký EntitySet cho User (entity gốc từ DB)
    odataBuilder.EntitySet<User>("ODataUsers"); // <--- Tên EntitySet là "ODataUsers" giống như route của ODataUsersController

    // Rất quan trọng: Đăng ký ReadUserDTO là một EntityType hoặc ComplexType
    // Việc này giúp OData hiểu cấu trúc của DTO cho các phép chiếu và metadata.
    // Nếu ReadUserDTO có một thuộc tính đóng vai trò là key (ví dụ UserId),
    // bạn có thể đăng ký nó là EntityType. Nếu không, là ComplexType.
    odataBuilder.EntityType<ReadUserDTO>(); // <--- Thêm dòng này

    return odataBuilder.GetEdmModel();
}

// 2. Add OData services
builder.Services.AddControllers().AddOData(options =>
{
    // Đăng ký route components cho ODataUsersController
    // Tiền tố "odata" phải khớp với [Route("odata/[controller]")] của ODataUsersController
    options.AddRouteComponents("odata", GetEdmModel()) // route: /odata/ODataUsers
        .Select()
        .Filter()
        .OrderBy()
        .Expand()
        .Count()
        .SetMaxTop(100);
});

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

// Authorization policies (cho phép Customer) để dùng trong Controller
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Customer", policy =>
    {
        policy.RequireRole("Customer");
    });
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
app.UseODataRouteDebug(); // Thêm dòng này vào đây

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting(); //✅ 1. Thêm dòng này để đảm bảo routing hoạt động

//✅ 2. CORS configuration ( nếu dùng SignalR thì cần CORS )

// ✅ 3. Phải đặt sau UseRouting và trước MapControllers
app.UseHttpsRedirection(); // dung để chuyển đổi HTTP sang HTTPS

//✅ 4. User authentication and authorization
app.UseAuthentication(); // ✅ Phải đặt trước UseAuthorization và sau UseRouting
app.UseAuthorization(); // ✅ Phải đặt sau UseAuthentication và trước MapControllers

// ✅5. Phải đặt sau UseRouting và trước MapHub
app.MapControllers();  // Map url /odata/ODataUsers/ đến ODataUsersController

//✅ 6. Map SignalR hubs (nếu có)

app.Run();
