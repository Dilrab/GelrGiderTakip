using ApiGelirGider.Services;
using ApiGelirGider.Services.Implementations;
using ApiGelirGider.Services.Interfaces;
using ApiGelirGider.WebApi.Context;
using ApiGelirGider.WebApi.Mappings;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1) Temel API servisleri
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2) AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// 3) Uygulama servisleri
builder.Services.AddScoped<IIncomeService, IncomeService>();
builder.Services.AddScoped<EExpenseService, ExpenseService>();
builder.Services.AddScoped<CCategoryService, CategoryService>();

// 4) Veritabanı — TEK kayıt ve null-koruması
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' bulunamadı. appsettings(.Environment).json içinde tanımlayın.");

builder.Services.AddDbContext<ApiContext>(options =>
    options.UseSqlServer(connectionString));

// 5) CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowUI", policy =>
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin());
});

var app = builder.Build();

// 6) Debug: EF’in gerçekten hangi connection string’i gördüğünü kontrol etmek için
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApiContext>();
    Console.WriteLine("EF ConnectionString => " + db.Database.GetDbConnection().ConnectionString);
    // İsterseniz: await db.Database.CanConnectAsync() ile canlı test yapabilirsiniz
}

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

