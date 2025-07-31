using ApiGelirGider.WebApi.DTOs.Income;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public class IncomeController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public IncomeController(IConfiguration configuration)
    {
        _configuration = configuration;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_configuration.GetSection("ApiConfig:BaseUrl").Value)
        };
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("/api/incomes");
        var json = await response.Content.ReadAsStringAsync();
        var incomes = JsonConvert.DeserializeObject<List<IncomeDto>>(json);
        return View(incomes);
    }
}
