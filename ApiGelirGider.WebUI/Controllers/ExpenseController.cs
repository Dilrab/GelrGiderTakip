using ApiGelirGider.DTOs.Expense;
using Microsoft.AspNetCore.Mvc;

namespace ApiGelirGider.WebUI.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly ILogger<ExpenseController> _logger;

        public ExpenseController(ILogger<ExpenseController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            var dto = new ExpenseDto();
            return View();
        }
    }
}













