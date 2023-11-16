using IdentityNative.Context;
using IdentityNative.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("String de conexão não foi encontrada.");

builder.Services.AddDbContext<Context>(options => options.UseSqlServer(connectionString));

builder.Services.AddTransient<TokenService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configurar o serviço de sessão
builder.Services.AddDistributedMemoryCache(); // Usando o cache em memória (pode ser substituído por outro provedor, como Redis)

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(8); // Tempo limite da sessão
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

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

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
