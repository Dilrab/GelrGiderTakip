using ApiGelirGider.DTOs.Category;
using ApiGelirGider.DTOs.Expense;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ApiGelirGider.WebUI.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly ILogger<ExpenseController> _logger;
        private readonly HttpClient _client;

        public ExpenseController(
            ILogger<ExpenseController> logger,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _client = httpClientFactory.CreateClient("myClient");
        }

        // ✅ Giderleri Listeleme – GET: /EXpense/Index
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("api/Expenses");
            var model = new ExpenseCreateDto();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var expenses = JsonConvert.DeserializeObject<List<ExpenseDto>>(json);
                model.ExpenseList = expenses;
            }

            return View(model);
        }

        // ✅ Gider Ekleme Formu – GET: /Expense/Add
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            var model = new ExpenseCreateDto();
            await LoadCategoriesAsync(model);

            return View(model);
        }

        // ✅ Gider Ekleme – POST: /Gider/Post
        [HttpPost]
        public async Task<IActionResult> Post(ExpenseCreateDto model)
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
                const string apiPath = "api/Expenses";
                HttpResponseMessage result;

                if (model.ExpenseId > 0)
                    result = await _client.PutAsync(apiPath, model.ToStringContent());
                else
                    result = await _client.PostAsync(apiPath, model.ToStringContent());

                var content = await result.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                    model.ResultMessage = "Gider başarıyla kaydedildi.";

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
        private async Task LoadCategoriesAsync(ExpenseCreateDto model)
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
