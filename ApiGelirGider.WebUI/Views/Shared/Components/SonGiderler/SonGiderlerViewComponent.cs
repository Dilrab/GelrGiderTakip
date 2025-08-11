using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using ApiGelirGider.DTOs;
using ApiGelirGider.DTOs.Expense; // Gider modelin burada olmalı

public class SonGiderlerViewComponent : ViewComponent
{
    private readonly HttpClient _httpClient;

    public SonGiderlerViewComponent(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var apiUrl = "https://localhost:5001/api/Gider/son5"; // API endpoint'in
        var sonGiderler = await _httpClient.GetFromJsonAsync<List<ExpenseCreateDto>>(apiUrl);
        return View(sonGiderler);
    }
}
