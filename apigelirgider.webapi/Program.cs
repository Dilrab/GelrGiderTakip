using ApiGelirGider.Services;
using ApiGelirGider.Services.Implementations;
using ApiGelirGider.Services.Interfaces;
using ApiGelirGider.WebApi.Context;
using ApiGelirGider.WebApi.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1) Temel API servisleri
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2) AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// 3) Uygulama servisleri (DI Container)
builder.Services.AddScoped<IIncomeService, IncomeService>();
builder.Services.AddScoped<EExpenseService, ExpenseService>();
builder.Services.AddScoped<CCategoryService, CategoryService>();

// 4) Veritabanı bağlantısı
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' bulunamadı. appsettings.json içinde tanımlayın.");

builder.Services.AddDbContext<ApiContext>(options =>
    options.UseSqlServer(connectionString));

// 5) CORS politikası
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowUI", policy =>
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin());
});

// 6) JWT Authentication yapılandırması
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwt = builder.Configuration.GetSection("Jwt");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["Key"]!))
        };
    });

// 7) Yetkilendirme servisi
builder.Services.AddAuthorization();

var app = builder.Build();

// 8) Debug amaçlı connection string yazdırma (isteğe bağlı)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApiContext>();
    Console.WriteLine("EF ConnectionString => " + db.Database.GetDbConnection().ConnectionString);
}

// 9) Geliştirme ortamı Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 10) Middleware sıralaması (doğru sıralama çok önemli)
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowUI");
app.UseAuthentication(); // 🔹 JWT doğrulama middleware’i
app.UseAuthorization();  // 🔹 Yetkilendirme middleware’i

// 11) Controller endpoint’leri
app.MapControllers();

// 12) Uygulamayı çalıştır
app.Run();
