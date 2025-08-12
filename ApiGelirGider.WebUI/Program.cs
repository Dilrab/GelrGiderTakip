using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// 1. Service kayýtlarý (Build() öncesi)
builder.Services.AddControllersWithViews();

// Session için memory cache servisi
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// CORS politikasý
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

// HttpClient tanýmý (adlandýrýlmýþ)
builder.Services.AddHttpClient("myClient", client =>
{
client.BaseAddress = new Uri("https://localhost:7080/");
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(
    new MediaTypeWithQualityHeaderValue("application/json"));
new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json");

}); 
var app = builder.Build();

// 2. Middleware tanýmlamalarý (Build() sonrasý)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Routing ve CORS
app.UseRouting();
app.UseCors("AllowAll");

// Session ve Authorization
app.UseSession();
app.UseAuthorization();

// Controller endpoint’leri
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
