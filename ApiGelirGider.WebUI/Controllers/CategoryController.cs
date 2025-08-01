using ApiGelirGider.DTOs.Category;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace ApiGelirGider.WebUI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly HttpClient _client;

        public CategoryController(ILogger<CategoryController> logger , IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _client = httpClientFactory.CreateClient("myClient");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add(CategoryDtoEdit? model)
        {

            if (model != null)
            {
                return View(model);
            }
            return View(new CategoryDtoEdit());
        }
        //public IActionResult Post()
        //{
        //    var dto = new CategoryDtoEdit();
        //    return View();
        //}

        public async Task<IActionResult> Post(CategoryDtoEdit model)
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
                    string categoryAPIPath = "api/Categories"; 

                    HttpResponseMessage result;
                    HttpContent content;


                    if (model.Id > 0)
                    {
                         result =  await _client.PutAsync(categoryAPIPath, model.ToStringContent());
                    }
                    else
                    {
                        result = await _client.PostAsync(categoryAPIPath, model.ToStringContent());
                    }

                    var contents = await  result.Content.ReadAsStringAsync();
                    if (!String.IsNullOrEmpty(contents))
                    {
                        //var response = JsonConvert.DeserializeObject(contents);
                       var  response = JsonConvert.DeserializeObject<CategoryDtoEdit>(contents) ;
                        model = response;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    model.ErrorMessage = "Bilin meyen bir hata oluþtu.";
                }
            }
       
            
            // return Json(new { response });

            return RedirectToAction("Add","Category", model);
        }
    }
}
