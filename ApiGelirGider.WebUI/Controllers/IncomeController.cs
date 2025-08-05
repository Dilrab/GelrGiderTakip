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
        public IncomeController(ILogger<IncomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _client = httpClientFactory.CreateClient("myClient");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add(IncomeDto? model)
        {
            if (model != null)
            {
                return View(model);
            }
            return View(new IncomeDto());
        }

        public async Task<IActionResult> Post(IncomeDto model)
        {

            if (!ModelState.IsValid)
            {
                model.ErrorMessage = "Model geçersiz kontrol edinmiz.";
                return View(model);

            }
            else
            {
                try
                {
                    string ıncomeAPIPath = "api/Incomes";

                    HttpResponseMessage result;
                    HttpContent content;


                    if (model.IncomeId > 0)
                    {
                        result = await _client.PutAsync(ıncomeAPIPath, model.ToStringContent());
                    }
                    else
                    {
                        result = await _client.PostAsync(ıncomeAPIPath, model.ToStringContent());
                    }

                    var contents = await result.Content.ReadAsStringAsync();
                    if (!String.IsNullOrEmpty(contents))
                    {
                        //var response = JsonConvert.DeserializeObject(contents);
                        var response = JsonConvert.DeserializeObject<IncomeDto>(contents);
                        model = response;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    model.ErrorMessage = "Bilin meyen bir hata oluştu.";
                }
               
            }


            // return Json(new { response });
        
            return RedirectToAction("Add", "Income", model);

        }
    }

}





