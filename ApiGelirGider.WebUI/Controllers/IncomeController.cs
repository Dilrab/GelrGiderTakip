using ApiGelirGider.DTOs.Category;
using ApiGelirGider.DTOs.Income;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ApiGelirGider.WebUI.Controllers
{
    public class IncomeController : Controller
    {
        private readonly ILogger<IncomeController> _logger;
        private readonly HttpClient _client;

        public IncomeController(
            ILogger<IncomeController> logger,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _client = httpClientFactory.CreateClient("myClient");
        }

        // ✅ Gelirleri Listeleme – GET: /Income/Index
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("api/Incomes");
            var model = new IncomeCreateDto();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var incomes = JsonConvert.DeserializeObject<List<IncomeDto>>(json);
                model.IncomeList = incomes;
            }

            return View(model);
        }

        // ✅ Gelir Ekleme Formu – GET: /Income/Add
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            var model = new IncomeCreateDto();
            await LoadCategoriesAsync(model);

            return View(model);
        }

        // ✅ Gelir Ekleme – POST: /Income/Post
        [HttpPost]
        public async Task<IActionResult> Post(IncomeCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync(model);
                model.ErrorMessage = "Model geçersiz, lütfen kontrol ediniz.";
                return View("Add", model);
            }

            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                await LoadCategoriesAsync(model);
                model.ErrorMessage = "Oturum bulunamadı.";
                return View("Add", model);
            }

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            try
            {
                const string apiPath = "api/Incomes";
                HttpResponseMessage result;

                if (model.IncomeId > 0)
                    result = await _client.PutAsync(apiPath, model.ToStringContent());
                else
                    result = await _client.PostAsync(apiPath, model.ToStringContent());

                var content = await result.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                    model.ResultMessage = "Gelir başarıyla kaydedildi.";

                await LoadCategoriesAsync(model);
                return View("Add", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gelir ekleme sırasında hata oluştu.");
                await LoadCategoriesAsync(model);
                model.ErrorMessage = "Sunucuya bağlanılamadı veya veri işlenemedi.";
                return View("Add", model);
            }
        }

        // ✅ Yardımcı metod: Kategorileri yükle
        private async Task LoadCategoriesAsync(IncomeCreateDto model)
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token)) return;

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("api/Categories");
            if (!response.IsSuccessStatusCode) return;

            var json = await response.Content.ReadAsStringAsync();
            var categories = JsonConvert.DeserializeObject<List<CategoryDtoEdit>>(json);

            model.CategoryList = categories.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.CategoryName
            }).ToList();
        }
    }
}
