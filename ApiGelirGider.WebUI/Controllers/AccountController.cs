using Microsoft.AspNetCore.Mvc;
using Dto.Dtos.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;              // PostAsJsonAsync & ReadFromJsonAsync
using System.Text.Json.Serialization;    // JsonPropertyName

namespace GelirGider.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IHttpClientFactory httpClientFactory,
                                 ILogger<AccountController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        // Giriş formu
        [HttpGet]
        public IActionResult Login() => View();

        // Giriş işlemi
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto loginDto)
        {
            try
            {
                // Program.cs'te kayıtlı named client'ı kullan
                var client = _httpClientFactory.CreateClient("myClient");

                // Sadece relative path gönder (BaseAddress devrede)
                var response = await client.PostAsJsonAsync("api/auth/login", loginDto);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Login başarısız. StatusCode: {Status}", response.StatusCode);
                    ViewBag.Error = "Giriş başarısız. Bilgilerinizi kontrol edin.";
                    return View();
                }

                var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
                if (string.IsNullOrWhiteSpace(result?.Token))
                {
                    ViewBag.Error = "Geçersiz yanıt alındı.";
                    return View();
                }

                HttpContext.Session.SetString("token", result.Token);
                return RedirectToAction("Index", "Home");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API'ye bağlanılamadı. BaseAddress/port veya profil hatası olabilir.");
                ViewBag.Error = "Sunucuya bağlanılamadı. Lütfen daha sonra tekrar deneyin.";
                return View();
            }
        }

        // Çıkış işlemi
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("token");
            return RedirectToAction("Login", "Account");
        }

        // TokenResponse sınıfı (API {"token": "..."} döndürüyor)
        public class TokenResponse
        {
            [JsonPropertyName("token")]
            public string Token { get; set; }
        }
    }
}
