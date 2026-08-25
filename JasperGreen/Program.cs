using Microsoft.EntityFrameworkCore;
using JasperGreen.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

//For future session state (if needed)
builder.Services.AddMemoryCache();
builder.Services.AddSession();

// Add services to the container.
builder.Services.AddRouting(options => {
    options.LowercaseUrls = true;
    options.AppendTrailingSlash = true;
});

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<JasperGreenDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("JasperGreen")));

//Can change password options
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<JasperGreenDbContext>()
    .AddDefaultTokenProviders();

//PdfMyHtml Service Registration
builder.Services.AddHttpClient<PdfMyHtmlService>();

//RazorToString Registration
builder.Services.AddScoped<RazorViewToStringRenderer>();

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

//Configure app to use authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>(); 
using (var scope = scopeFactory.CreateScope())
{
	await ConfigureIdentity.CreateAdminUserAsync(scope.ServiceProvider);
}

//must be called before routes are mapped - Cole
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();