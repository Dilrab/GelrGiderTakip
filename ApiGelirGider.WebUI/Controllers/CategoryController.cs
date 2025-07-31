using ApiGelirGider.WebApi.DTOs.Category;
using ApiGelirGider.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ApiGelirGider.WebUI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ILogger<CategoryController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Add()
        {
            var dto = new CategoryCreateDto();
            return View();
        }


    }
}
