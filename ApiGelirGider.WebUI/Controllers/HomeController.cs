using ApiGelirGider.DTOs.Expense;
using ApiGelirGider.DTOs.Income;
using ApiGelirGider.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ApiGelirGider.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        // ? Ana sayfa: Son 5 gider + Son 5 gelir
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("myClient");

            var expenseResponse = await client.GetAsync("api/expenses/last5");
            var incomeResponse = await client.GetAsync("api/incomes/last5");

            var expenses = new List<ExpenseDto>();
            var incomes = new List<IncomeDto>();

            if (expenseResponse.IsSuccessStatusCode)
            {
                var expenseJson = await expenseResponse.Content.ReadAsStringAsync();
                expenses = JsonConvert.DeserializeObject<List<ExpenseDto>>(expenseJson);
            }
            else
            {
                _logger.LogWarning("Gider API'den veri alýnamadý: " + expenseResponse.StatusCode);
            }

            if (incomeResponse.IsSuccessStatusCode)
            {
                var incomeJson = await incomeResponse.Content.ReadAsStringAsync();
                incomes = JsonConvert.DeserializeObject<List<IncomeDto>>(incomeJson);
            }
            else
            {
                _logger.LogWarning("Gelir API'den veri alýnamadý: " + incomeResponse.StatusCode);
            }
            var incomeTotalResponse = await client.GetAsync("api/incomes/total");
            var expenseTotalResponse = await client.GetAsync("api/expenses/total");

            decimal totalIncome = 0, totalExpense = 0;

            if (incomeTotalResponse.IsSuccessStatusCode)
            {
                var json = await incomeTotalResponse.Content.ReadAsStringAsync();
                totalIncome = JsonConvert.DeserializeObject<decimal>(json);
            }

            if (expenseTotalResponse.IsSuccessStatusCode)
            {
                var json = await expenseTotalResponse.Content.ReadAsStringAsync();
                totalExpense = JsonConvert.DeserializeObject<decimal>(json);
            }

            var model = new DashboardViewModel
            {
                LastExpenses = expenses,
                LastIncomes = incomes,
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
            };

            ViewBag.TotalIncome = model.TotalIncome;
            ViewBag.TotalExpense = model.TotalExpense;

            return View(model); // Views/Home/Index.cshtml
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

