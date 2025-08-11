using ApiGelirGider.DTOs.Expense;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using ApiGelirGider.DTOs.Category;

namespace ApiGelirGider.WebUI.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly ILogger<ExpenseController> _logger;
        private readonly HttpClient _client;

        // ✅ Tek constructor – Hem logger hem HttpClientFactory alıyor
        public ExpenseController(
            ILogger<ExpenseController> logger,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _client = httpClientFactory.CreateClient("myClient");
        }

        // 🧾 Giderleri Listeleme
        public async Task<IActionResult> Index()
        {
            var response = await _client.GetAsync("api/Expenses");
            if (!response.IsSuccessStatusCode)
                return View(new List<ExpenseDto>());

            var json = await response.Content.ReadAsStringAsync();
            var expenses = JsonConvert.DeserializeObject<List<ExpenseDto>>(json);
            return View(expenses);
        }

        // ➕ Gider Ekleme Formu
        [HttpGet]
        public IActionResult Add(ExpenseDto? model)
        {
            return View(model ?? new ExpenseDto());
        }

        // 📤 Gider Ekleme / Güncelleme
        [HttpPost]
        public async Task<IActionResult> Post(ExpenseDto model)
        {
            if (!ModelState.IsValid)
            {
                model.ErrorMessage = "Model geçersiz, lütfen kontrol ediniz.";
                return View(model);
            }

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
                    model = JsonConvert.DeserializeObject<ExpenseDto>(content)!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gider ekleme/güncelleme sırasında hata oluştu.");
                ModelState.AddModelError("", ex.Message);
                model.ErrorMessage = "Bilinmeyen bir hata oluştu.";
                return View(model);
            }

            return RedirectToAction("Add", "Expense", new { id = model.ExpenseId });
        }

        // 🧮 Son 5 Gider Listeleme
        [HttpGet("Last5")]
        public async Task<IActionResult> Last5()
        {
            var response = await _client.GetAsync("api/expenses/last5");
            if (!response.IsSuccessStatusCode)
                return View(new List<ExpenseDto>());

            var json = await response.Content.ReadAsStringAsync();
            var last5Expenses = JsonConvert.DeserializeObject<List<ExpenseDto>>(json);
            return View(last5Expenses);
        }
    }
}
