using ApiGelirGider.DTOs.Category;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace ApiGelirGider.WebUI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly HttpClient _client;

        public CategoryController(
            ILogger<CategoryController> logger,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _client = httpClientFactory.CreateClient("myClient");
        }

        // Kategori listesi � GET: /Category/Index
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("api/Categories");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Kategori listesi al�namad�. StatusCode: {Status}", response.StatusCode);
                return View(new List<CategoryDtoEdit>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var categories = JsonConvert.DeserializeObject<List<CategoryDtoEdit>>(json);
            return View(categories);
        }

        // Kategori ekleme formu � GET: /Category/Add
        [HttpGet]
        public IActionResult Add()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            return View(new CategoryDtoEdit());
        }

        // Kategori ekleme/g�ncelleme � POST: /Category/Post
        [HttpPost]
        public async Task<IActionResult> Post(CategoryDtoEdit model)
        {
            if (!ModelState.IsValid)
            {
                model.ErrorMessage = "Model ge�ersiz, l�tfen kontrol ediniz.";
                return View("Add", model);
            }

            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                model.ErrorMessage = "Oturum bulunamad�.";
                return View("Add", model);
            }

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            try
            {
                const string apiPath = "api/Categories";
                HttpResponseMessage result;

                if (model.Id > 0)
                    result = await _client.PutAsync(apiPath, model.ToStringContent());
                else
                    result = await _client.PostAsync(apiPath, model.ToStringContent());

                var content = await result.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                    model = JsonConvert.DeserializeObject<CategoryDtoEdit>(content)!;

                model.ResultMessage = "Kategori ba�ar�yla kaydedildi.";
                return View("Add", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori ekleme/g�ncelleme s�ras�nda hata olu�tu.");
                model.ErrorMessage = "Sunucuya ba�lan�lamad� veya veri i�lenemedi.";
                return View("Add", model);
            }
        }
    }
}
