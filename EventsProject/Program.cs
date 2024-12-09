using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using ServiceStack;

var builder = WebApplication.CreateBuilder(args);

// Session configuration
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session süresi
    options.Cookie.HttpOnly = true; // Güvenlik için HttpOnly
    options.Cookie.IsEssential = true; // Session'ý temel ihtiyaç olarak ayarla
});

// Register services for business and data layers
builder.Services.AddScoped<IArtistDal, EfArtistDal>();
builder.Services.AddScoped<IArtistService, ArtistManager>();
builder.Services.AddScoped<IEventService, EventManager>();
builder.Services.AddScoped<IEventDal, EfEventDal>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<EfCategoryDal, EfCategoryDal>();
builder.Services.AddScoped<IGenericDal<Category>, EfCategoryDal>();
builder.Services.AddScoped<ITicketService, TicketManager>();
builder.Services.AddScoped<ITicketDal, EfTicketDal>();
builder.Services.AddScoped<IGenericDal<Ticket>, EfTicketDal>();
builder.Services.AddScoped<IGenericDal<Artist>, EfArtistDal>();
builder.Services.AddScoped<IGenericDal<Message>, EfMessageDal>();
builder.Services.AddScoped<IMessageDal, EfMessageDal>();
builder.Services.AddScoped<IMessageService, MessageManager>();
builder.Services.AddScoped<ICityService, CityManager>();
builder.Services.AddScoped<ICityDal, EfCityDal>();
builder.Services.AddScoped<IGenericDal<City>, EfCityDal>();

// Add controllers and views
builder.Services.AddControllersWithViews();

// Add DbContext and Identity configuration
builder.Services.AddDbContext<Context>();
builder.Services.AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<Context>();

// Add authentication and authorization
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Member/Login";  // Giriþ sayfasý
        options.LogoutPath = "/Member/Logout"; // Çýkýþ sayfasý
    });

builder.Services.AddMvc();

var app = builder.Build();

// Configure middlewares
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();

// Use session middleware here
app.UseSession();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
