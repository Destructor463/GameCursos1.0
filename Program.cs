using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GameCursos.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHealthChecks();

// Add services to the container.
var connectionString=Environment.GetEnvironmentVariable("RENDER_POSTGRES_CONNECTION");
if (string.IsNullOrEmpty(connectionString))
{
// Add services to the container.
    connectionString = builder.Configuration.GetConnectionString("PostgresSQLConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    //options.UseSqlite(connectionString));
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.MapHealthChecks("/health");

app.Run();
