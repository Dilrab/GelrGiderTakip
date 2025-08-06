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

        // ?. Constructor � HttpClientFactory ile �myClient� �rne�ini al�yoruz
        public CategoryController(
            ILogger<CategoryController> logger,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _client = httpClientFactory.CreateClient("myClient");
        }

        // ?. Gelirleri Listeleme � GET: /Income/Index
        public async Task<IActionResult> Index()
        {
            var response = await _client.GetAsync("api/Categories");
            if (!response.IsSuccessStatusCode)
            {
                // Hata ya da bo� liste durumunda
                return View(new List<CategoryDtoEdit>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var categories = JsonConvert.DeserializeObject<List<CategoryDtoEdit>>(json);
            return View(categories);
        }

        // ?. Gelir Ekleme Formu � GET: /Income/Add
        [HttpGet]
        public IActionResult Add(CategoryController? model)
        {
            if (model != null)
                return View(model);

            return View(new CategoryDtoEdit());
        }

        // ?. Gelir Ekleme / G�ncelleme � POST: /Income/Post
        [HttpPost]
        public async Task<IActionResult> Post(CategoryDtoEdit model)
        {
            // ?1. Model ge�erlili�i kontrol�
            if (!ModelState.IsValid)
            {
                model.ErrorMessage = "Model ge�ersiz, l�tfen kontrol ediniz.";
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

                // ?3. API yan�t�n� okuyup modele yans�t�yoruz
                var content = await result.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                    model = JsonConvert.DeserializeObject<CategoryDtoEdit>(content)!;
            }
            catch (Exception ex)
            {
                // ?4. Hata yakalama ve loglama
                _logger.LogError(ex, "Gelir ekleme/g�ncelleme s�ras�nda hata olu�tu.");
                ModelState.AddModelError("", ex.Message);
                model.ErrorMessage = "Bilinmeyen bir hata olu�tu.";
                return View(model);
            }

            // ?5. ��lem ba�ar�l�ysa tekrar Add sayfas�na y�nlendiriyoruz
            return RedirectToAction("Add", "Category", model);
        }
    }
}

