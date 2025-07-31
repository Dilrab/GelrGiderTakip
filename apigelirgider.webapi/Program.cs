using apigelirgider.webapi.Mappings;
using ApiGelirGider.WebApi.Context;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;



var builder = WebApplication.CreateBuilder(args);

//entity d�n���m� i�in gerekli
builder.Services.AddAutoMapper(typeof(MappingProfile));

//Ba�alant� i�n Cors engelini kald�r�r
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowUI",
        policy => policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin()); // UI projesi hangi portta �al���yorsa onu belirtebilirsin
});

app.UseCors("AllowUI");



// servis tan�mlamas� 
builder.Services.AddDbContext<ApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
