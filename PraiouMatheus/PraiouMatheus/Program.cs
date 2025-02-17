using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados
builder.Services.AddDbContext<PraiouMatheus.Data.AppCont>(options =>
{
    options.UseSqlServer(builder
        .Configuration
        .GetConnectionString("PraiouProj"));
});

// Adicionar suporte à sessão e cache distribuído
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configuração de autenticação com cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuario/Login";  // Caminho para login
        options.LogoutPath = "/Usuario/Logout"; // Caminho para logout
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Tempo de expiração do cookie
    });

// Adicionar os serviços de controlador e views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configuração do pipeline de requisição
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Habilitar autenticação e autorização
app.UseAuthentication();  // Adicionando autenticação
app.UseAuthorization();

app.UseSession();  // Adicionando middleware de sessão

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
