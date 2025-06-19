using Service.BaseService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session tồn tại trong 30 phút
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// HttpClient
builder.Services.AddHttpClient<GatewayHttpClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7112/");
});

// Dependency Injection
builder.Services.AddScoped<Service.Interfaces.IUserService, Service.Services.UserService>();

builder.Services.AddScoped<Service.Services.EmailService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession(); // Sử dụng Session

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
