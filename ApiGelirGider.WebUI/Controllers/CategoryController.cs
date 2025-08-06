using ApiGelirGider.DTOs.Category;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using ApiGelirGider.DTOs.Category;

namespace ApiGelirGider.WebUI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly HttpClient _client;

        // ?. Constructor – HttpClientFactory ile “myClient” örneðini alýyoruz
        public CategoryController(
            ILogger<CategoryController> logger,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _client = httpClientFactory.CreateClient("myClient");
        }

        // ?. Gelirleri Listeleme – GET: /Income/Index
        public async Task<IActionResult> Index()
        {
            var response = await _client.GetAsync("api/Categories");
            if (!response.IsSuccessStatusCode)
            {
                // Hata ya da boþ liste durumunda
                return View(new List<CategoryDtoEdit>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var categories = JsonConvert.DeserializeObject<List<CategoryDtoEdit>>(json);
            return View(categories);
        }

        // ?. Gelir Ekleme Formu – GET: /Income/Add
        [HttpGet]
        public IActionResult Add(CategoryController? model)
        {
            if (model != null)
                return View(model);

            return View(new CategoryDtoEdit());
        }

        // ?. Gelir Ekleme / Güncelleme – POST: /Income/Post
        [HttpPost]
        public async Task<IActionResult> Post(CategoryDtoEdit model)
        {
            // ?1. Model geçerliliði kontrolü
            if (!ModelState.IsValid)
            {
                model.ErrorMessage = "Model geçersiz, lütfen kontrol ediniz.";
                return View(model);
            }

            try
            {
                const string apiPath = "api/Categories";
                HttpResponseMessage result;

                // ?2. POST mu PUT mu karar veriyoruz
                if (model.Id > 0)
                    result = await _client.PutAsync(apiPath, model.ToStringContent());
                else
                    result = await _client.PostAsync(apiPath, model.ToStringContent());

                // ?3. API yanýtýný okuyup modele yansýtýyoruz
                var content = await result.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                    model = JsonConvert.DeserializeObject<CategoryDtoEdit>(content)!;
            }
            catch (Exception ex)
            {
                // ?4. Hata yakalama ve loglama
                _logger.LogError(ex, "Gelir ekleme/güncelleme sýrasýnda hata oluþtu.");
                ModelState.AddModelError("", ex.Message);
                model.ErrorMessage = "Bilinmeyen bir hata oluþtu.";
                return View(model);
            }

            // ?5. Ýþlem baþarýlýysa tekrar Add sayfasýna yönlendiriyoruz
            return RedirectToAction("Add", "Category", model);
        }
    }
}

