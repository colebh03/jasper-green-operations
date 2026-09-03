using Microsoft.EntityFrameworkCore;
using JasperGreen.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

//Register session services for application state that may be needed across requests
builder.Services.AddMemoryCache();
builder.Services.AddSession();

// Configure lowercase URLs with trailing slashes
builder.Services.AddRouting(options => {
    options.LowercaseUrls = true;
    options.AppendTrailingSlash = true;
});

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<JasperGreenDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("JasperGreen")));

// Configure ASP.NET Core Identity with EF Core-backed users and roles
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<JasperGreenDbContext>()
    .AddDefaultTokenProviders();

// Register the external PDF generation service
builder.Services.AddHttpClient<PdfMyHtmlService>();

// Register Razor view rendering used to convert invoice views into HTML
builder.Services.AddScoped<RazorViewToStringRenderer>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");    
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Seed the configured administrator account and role at application startup
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>(); 

using (var scope = scopeFactory.CreateScope())
{
	await ConfigureIdentity.CreateAdminUserAsync(scope.ServiceProvider);
}

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();