using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);






builder.Services.AddScoped<IArtistDal, EfArtistDal>();
builder.Services.AddScoped<IArtistService, ArtistManager>();


builder.Services.AddScoped<IEventService, EventManager>();
builder.Services.AddScoped<IEventDal, EfEventDal>();



builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<EfCategoryDal, EfCategoryDal>();
builder.Services.AddScoped<IGenericDal<Category>, EfCategoryDal>(); // Register IGenericDal<Category>

builder.Services.AddScoped<ITicketService, TicketManager>(); // Register ITicketService with TicketManager implementation
builder.Services.AddScoped<ITicketDal, EfTicketDal>();
builder.Services.AddScoped<IGenericDal<Ticket>, EfTicketDal>();
builder.Services.AddScoped<IGenericDal<Artist>, EfArtistDal>();

//message
builder.Services.AddScoped<IGenericDal<Message>, EfMessageDal>();
builder.Services.AddScoped<IMessageDal, EfMessageDal>(); // Ayrıca ITicketDal'i de register ediyoruz
builder.Services.AddScoped<IMessageService, MessageManager>();
// City
builder.Services.AddScoped<ICityService, CityManager>();
builder.Services.AddScoped<ICityDal, EfCityDal>(); // ICityDal'i de doğru şekilde kaydediyoruz
builder.Services.AddScoped<IGenericDal<City>, EfCityDal>();
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<Context>();
builder.Services.AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<Context>();

// ArtistManager'ı IArtistService türüne kaydedin

builder.Services.AddControllersWithViews();


builder.Services.AddMvc();


var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();

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
