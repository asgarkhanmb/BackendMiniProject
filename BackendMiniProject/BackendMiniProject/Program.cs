using BackendMiniProject.Data;
using BackendMiniProject.Services.Interfaces;
using BackendMiniProject.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<AppDbContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();




builder.Services.AddScoped<ISliderService, SliderService>();
builder.Services.AddScoped<IInformationService, InformationService>();






builder.Services.Configure<IdentityOptions>(opt =>
{
    opt.Password.RequiredUniqueChars = 1;
    opt.Password.RequireNonAlphanumeric = true;
    opt.Password.RequireDigit = true;
    opt.Password.RequireLowercase = true;
    opt.Password.RequireUppercase = true;


    opt.User.RequireUniqueEmail = true;

    opt.SignIn.RequireConfirmedEmail = true;
});







var app = builder.Build();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

