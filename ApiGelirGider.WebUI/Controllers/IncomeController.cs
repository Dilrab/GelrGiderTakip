using ApiGelirGider.DTOs.Income;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using ApiGelirGider.DTOs.Category;

namespace ApiGelirGider.WebUI.Controllers
{
    public class IncomeController : Controller
    {
        private readonly ILogger<IncomeController> _logger;
        private readonly HttpClient _client;

        // α. Constructor – HttpClientFactory ile “myClient” örneğini alıyoruz
        public IncomeController(
            ILogger<IncomeController> logger,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _client = httpClientFactory.CreateClient("myClient");
        }

        // β. Gelirleri Listeleme – GET: /Income/Index
        public async Task<IActionResult> Index()
        {
            var response = await _client.GetAsync("api/Incomes");
            if (!response.IsSuccessStatusCode)
            {
                // Hata ya da boş liste durumunda
                return View(new List<IncomeDto>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var incomes = JsonConvert.DeserializeObject<List<IncomeDto>>(json);
            return View(incomes);
        }

        // γ. Gelir Ekleme Formu – GET: /Income/Add
        [HttpGet]
        public IActionResult Add(IncomeDto? model)
        {
            if (model != null)
                return View(model);

            return View(new IncomeDto());
        }

        // δ. Gelir Ekleme / Güncelleme – POST: /Income/Post
        [HttpPost]
        public async Task<IActionResult> Post(IncomeDto model)
        {
            // δ1. Model geçerliliği kontrolü
            if (!ModelState.IsValid)
            {
                model.ErrorMessage = "Model geçersiz, lütfen kontrol ediniz.";
                return View(model);
            }

            try
            {
                const string apiPath = "api/Incomes";
                HttpResponseMessage result;

                // δ2. POST mu PUT mu karar veriyoruz
                if (model.IncomeId > 0)
                    result = await _client.PutAsync(apiPath, model.ToStringContent());
                else
                    result = await _client.PostAsync(apiPath, model.ToStringContent());

                // δ3. API yanıtını okuyup modele yansıtıyoruz
                var content = await result.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                    model = JsonConvert.DeserializeObject<IncomeDto>(content)!;
            }
            catch (Exception ex)
            {
                // δ4. Hata yakalama ve loglama
                _logger.LogError(ex, "Gelir ekleme/güncelleme sırasında hata oluştu.");
                ModelState.AddModelError("", ex.Message);
                model.ErrorMessage = "Bilinmeyen bir hata oluştu.";
                return View(model);
            }

            // δ5. İşlem başarılıysa tekrar Add sayfasına yönlendiriyoruz
            return RedirectToAction("Add", "Income", model);
        }
    }
}
