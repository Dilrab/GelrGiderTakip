using ApiGelirGider.DTOs.Expense;
using ApiGelirGider.DTOs.Income;
using ApiGelirGider.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        // Ana sayfa: Son 5 gider + Son 5 gelir + toplamlar
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrWhiteSpace(token))
            {
                return RedirectToAction("Login", "Account");
            }

            var client = _httpClientFactory.CreateClient("myClient");
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // API çaðrýlarýný paralel baþlat
            var expenseTask = client.GetAsync("api/expenses/last5");
            var incomeTask = client.GetAsync("api/incomes/last5");
            var incomeTotalTask = client.GetAsync("api/incomes/total");
            var expenseTotalTask = client.GetAsync("api/expenses/total");

            await Task.WhenAll(expenseTask, incomeTask, incomeTotalTask, expenseTotalTask);

            var expenses = new List<ExpenseDto>();
            var incomes = new List<IncomeDto>();
            decimal totalIncome = 0, totalExpense = 0;

            if (expenseTask.Result.IsSuccessStatusCode)
            {
                var json = await expenseTask.Result.Content.ReadAsStringAsync();
                expenses = JsonConvert.DeserializeObject<List<ExpenseDto>>(json);
            }

            if (incomeTask.Result.IsSuccessStatusCode)
            {
                var json = await incomeTask.Result.Content.ReadAsStringAsync();
                incomes = JsonConvert.DeserializeObject<List<IncomeDto>>(json);
            }

            if (incomeTotalTask.Result.IsSuccessStatusCode)
            {
                var json = await incomeTotalTask.Result.Content.ReadAsStringAsync();
                totalIncome = JsonConvert.DeserializeObject<decimal>(json);
            }

            if (expenseTotalTask.Result.IsSuccessStatusCode)
            {
                var json = await expenseTotalTask.Result.Content.ReadAsStringAsync();
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

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
     

    }
}
