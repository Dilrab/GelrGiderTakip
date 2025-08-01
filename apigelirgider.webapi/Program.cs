﻿using ApiGelirGider.WebApi.Context;
using ApiGelirGider.WebApi.Mappings;
using ApiGelirGider.WebApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ? Servis Tanımlamaları
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ?? AutoMapper - DTO dönüşümü
builder.Services.AddAutoMapper(typeof(MappingProfile));
//Program iiçne kayıt
builder.Services.AddScoped<IncomeService>();

// ?? CORS Ayarı (UI projesi ile bağlantı için)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowUI",
        policy => policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin()); // Gerekirse .WithOrigins("http://localhost:7027") şeklinde kısıtlayabilirsin
});

//Cors izni

// ?? Veritabanı bağlantısı
builder.Services.AddDbContext<ApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ⚙️ Uygulamayı oluşturuyoruz
var app = builder.Build();

// ✅ Middleware pipeline işlemleri
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowUI");
app.UseAuthorization();
app.MapControllers();

app.Run();
